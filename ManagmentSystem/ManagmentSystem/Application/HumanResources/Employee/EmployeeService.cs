using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using Domain.Common.Output;
using Domain.HumanResources;
using Domain.Infrastructure.ORM;
using Infrastructure.AppException;
using Infrastructure.Helper;
using Infrastructure.Service;
using Infrastructure.Static;

using Microsoft.EntityFrameworkCore;

namespace Application.HumanResources;

public class EmployeeService : MasterService, IEmployee, IScopped
{
    public EmployeeService(IMapper mapper, IHelper helper, DBContext context) : base(mapper, helper, context)
    {
    }

    public async Task<EmployeeOut> Get(int id)
    {
        Employee? data = await _context.Employee.FindAsync(id);

        if (data == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }

        return _mapper.Map<EmployeeOut>(data);
    }

    public async Task<FilterResultOut<EmployeeDataOut>> GetData(EmployeeFilter filter)
    {
        IQueryable<EmployeeDataOut> query =


            from emp in _context.Employee.AsNoTracking()

            where (string.IsNullOrWhiteSpace(filter.Name)) || emp.Name.ToLower().Contains(filter.Name.ToLower())
            && (string.IsNullOrWhiteSpace(filter.Gender)) || emp.Gender.ToLower().Contains(filter.Gender.ToLower())
             && (string.IsNullOrWhiteSpace(filter.Phone)) || emp.Phone.ToLower().Contains(filter.Phone.ToLower())
             && (filter.FromBirthday == null || emp.Birthday == null || emp.Birthday >= filter.FromBirthday)

              && (filter.ToBirthday == null || emp.Birthday >= filter.ToBirthday)
              && (string.IsNullOrWhiteSpace(filter.JobTitle)) || emp.JobTitle.ToLower().Contains(filter.JobTitle.ToLower())
               && (filter.FromJobRank == null || emp.JobRank >= filter.FromJobRank)
               && (filter.ToJobRank == null || emp.JobRank >= filter.ToJobRank)

                && (filter.FromCreateDate == null || emp.CreateDate >= filter.FromCreateDate)
                  && (filter.ToCreateDate == null || emp.CreateDate <= filter.ToCreateDate)
                  && (filter.FromUpdateDate == null || emp.UpdateDate == null || emp.UpdateDate >= filter.FromUpdateDate)
                  && (filter.ToUpdateDate == null || emp.UpdateDate == null || emp.UpdateDate <= filter.ToUpdateDate)
                  && (filter.FromRemoveDate == null || emp.RemoveDate == null || emp.RemoveDate >= filter.FromRemoveDate)
                  && (filter.ToRemoveDate == null || emp.RemoveDate == null || emp.RemoveDate <= filter.ToRemoveDate)
                  && (filter.IsRemoved == emp.IsRemoved)

            select _mapper.Map<EmployeeDataOut>(emp);


        return new FilterResultOut<EmployeeDataOut>(filter.PageSize, await query.CountAsync(), await query.ToListAsync());



    }
    public async Task<List<AutoComplete>> GetAutoComplete(string? term)
    {
        return await _context.Employee.AsNoTracking()
            .Where(x => string.IsNullOrWhiteSpace(term) || x.Name.ToLower().Contains(term.ToLower()))
            .Select(x => new AutoComplete { Key = x.Id, Value = x.Name })
            .ToListAsync();
    }


    public async Task<EmployeeOut> Create(EmployeeModel model)
    {
        if (await _context.Employee.AsNoTracking().AnyAsync(x => x.Name.ToLower().Equals(model.Name.ToLower())))
        {
            throw new DuplicateException(nameof(model.Name));
        }

        Employee employee = _mapper.Map<Employee>(model);

        employee.IsRemoved = false;
        employee.CreateUserId = 0;
        employee.CreateDate = DateTime.UtcNow.AddHours(3);

        await _context.Employee.AddAsync(employee);

        await _context.SaveChangesAsync();

        return _mapper.Map<EmployeeOut>(employee);
    }


    public async Task Update(EmployeeModel model)
    {
        Employee? employee = await _context.Employee.FindAsync(model.Id);

        if (employee == null)
        {
            throw new KeyNotFoundException(nameof(model.Id));
        }

        _mapper.Map(model, employee);

        employee.UpdateUserId = 0;
        employee.UpdateDate = DateTime.UtcNow.AddHours(3);

        _context.Update(employee);

        await _context.SaveChangesAsync();
    }



    public async Task Remove(int id)
    {
        Employee? employee = await _context.Employee.FindAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }

        if (employee.IsRemoved)
        {
            await PermanentRemove(id);
        }
        else
        {
            employee.IsRemoved = true;
            employee.RemoveUserId = 0;
            employee.RemoveDate = DateTime.UtcNow.AddHours(3);

            _context.Update(employee);

            await _context.SaveChangesAsync();
        }
    }

    public async Task PermanentRemove(int id)
    {
        Employee? employee = await _context.Employee.FindAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }

        _context.Remove(employee);

        await _context.SaveChangesAsync();
    }

    public async Task Restore(int id)
    {
        Employee? employee = await _context.Employee.FindAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException(nameof(id));
        }

        employee.IsRemoved = false;
        employee.CreateUserId = 0;
        employee.CreateDate = DateTime.UtcNow.AddHours(3);

        _context.Update(employee);

        await _context.SaveChangesAsync();
    }

      





}



