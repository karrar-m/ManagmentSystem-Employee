﻿namespace Domain.Infrastructure.ORM;


public enum ConnectionType
{
    SqlServer,
    PostgreSQL,
}


public class DBConn
{

    public static readonly ConnectionType ConnectionType = ConnectionType.SqlServer;

    public static string ConnectionString => ConnectionType switch
    {
        ConnectionType.SqlServer => "Server=(localdb)\\MSSQLLocalDB; Database=ManagmentSystem; " +
            "Trusted_Connection=True; MultipleActiveResultSets=true;",

      //  ConnectionType.PostgreSQL => "Server=localhost; Port=5432; Database=ManagmentSystem; " +
         //   "User Id=postgres; Password=postgres;",

        _ => "",
    };

}
