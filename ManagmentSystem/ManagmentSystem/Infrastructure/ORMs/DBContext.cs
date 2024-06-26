﻿using Domain.HumanResources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Domain.Infrastructure.ORM;

public class DBContext: DbContext
{


    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeMap());
    }

    public DbSet<Employee> Employee { get; set; }
}


