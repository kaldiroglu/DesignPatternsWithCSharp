namespace dev.kaldiroglu.CreationalIntroduction.Db.Problem;

/// <summary>Five connections set up with property assignments. The last has no database, and nothing says so.</summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/CreationalIntroduction.Demo -- db-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        // Scenario 1: Basic local development connection
        DatabaseConnection devConn = new DatabaseConnection();
        devConn.Host = "localhost";
        devConn.Port = 3306;
        devConn.Database = "myapp_dev";
        devConn.Username = "dev_user";
        devConn.Password = "dev_pass";

        // Scenario 2: Production connection with custom security and timeouts
        DatabaseConnection prodConn = new DatabaseConnection();
        prodConn.Host = "prod-db-cluster.company.com";
        prodConn.Port = 5432;  // PostgreSQL
        prodConn.Database = "myapp_production";
        prodConn.Username = "prod_user";
        prodConn.Password = "complex_secure_password";
        prodConn.UseSSL = true;
        prodConn.ConnectionTimeout = 10000;
        prodConn.ReadTimeout = 120000;
        prodConn.ConnectionPool = "HikariCP";
        prodConn.EnableLogging = true;

        // Scenario 3: Testing connection with specific charset and no SSL
        DatabaseConnection testConn = new DatabaseConnection();
        testConn.Host = "test-server";
        testConn.Database = "test_db";
        testConn.Username = "test_user";
        testConn.Password = "test_pass";
        testConn.UseSSL = false;
        testConn.Charset = "UTF-8";
        testConn.AutoReconnect = false;

        // Scenario 4: Analytics connection with custom timezone and logging
        DatabaseConnection analyticsConn = new DatabaseConnection();
        analyticsConn.Host = "analytics-db";
        analyticsConn.Database = "warehouse";
        analyticsConn.Username = "analytics_user";
        analyticsConn.Password = "analytics_pass";
        analyticsConn.Timezone = "UTC";
        analyticsConn.EnableLogging = true;
        analyticsConn.LogLevel = "DEBUG";
        analyticsConn.ReadTimeout = 300000; // 5 minutes for long queries

        // Scenario 5: a setter call is forgotten, and nothing says so
        DatabaseConnection forgotten = new DatabaseConnection();
        forgotten.Host = "localhost";
        forgotten.Port = 3306;
        forgotten.Username = "dev_user";
        forgotten.Password = "dev_pass";

        Console.WriteLine(devConn);
        Console.WriteLine(prodConn);
        Console.WriteLine(testConn);
        Console.WriteLine(analyticsConn);
        Console.WriteLine(forgotten);
    }
}
