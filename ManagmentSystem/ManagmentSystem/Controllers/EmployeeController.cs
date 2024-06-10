using Application.HumanResources;
using Domain.HumanResources;
using ManagmentSystem.Domain.Security.User.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployee _EmployeeService;


     public EmployeeController(IEmployee EmployeeService)
    {
        _EmployeeService = EmployeeService;
    }

    [HttpPost] 
    
    public async Task<IActionResult> Create(EmployeeModel model)

    {

        await _EmployeeService.Create(model);
        return Ok();




    }
    }
