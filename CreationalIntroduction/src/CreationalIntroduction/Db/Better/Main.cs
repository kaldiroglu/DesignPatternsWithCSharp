namespace dev.kaldiroglu.CreationalIntroduction.Db.Better;

/// <summary>Four connections made through the builder, and one the builder rejects.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- db-better</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        DatabaseConnection devConn =
                DatabaseConnection.CreateBuilder("localhost", "myapp_dev", "dev_user", "dev_pass")
                        .Build();

        DatabaseConnection prodConn =
                DatabaseConnection.CreateBuilder("prod-db-cluster.company.com", "myapp_production", "prod_user", "complex_secure_password")
                        .Port(5432) // PostgreSQL
                        .UseSSL(true)
                        .ConnectionTimeout(10000)
                        .ReadTimeout(120000)
                        .ConnectionPool("HikariCP")
                        .EnableLogging(true)
                        .Build();

        DatabaseConnection testConn =
                DatabaseConnection.CreateBuilder("test-server", "test_db", "test_user", "test_pass")
                        .UseSSL(false)
                        .Charset("UTF-8")
                        .AutoReconnect(false)
                        .Build();

        DatabaseConnection analyticsConn =
                DatabaseConnection.CreateBuilder("analytics-db", "warehouse", "analytics_user", "analytics_pass")
                        .Timezone("UTC")
                        .EnableLogging(true)
                        .LogLevel("DEBUG")
                        .ReadTimeout(300000) // 5 minutes for long queries
                        .Build();

        Console.WriteLine(devConn);
        Console.WriteLine(prodConn);
        Console.WriteLine(testConn);
        Console.WriteLine(analyticsConn);

        // A required value is missing, so no object is created.
        try
        {
            DatabaseConnection.CreateBuilder("localhost", "", "dev_user", "dev_pass").Build();
        }
        catch (ArgumentException e)
        {
            Console.WriteLine("Rejected: " + e.Message);
        }
    }
}
