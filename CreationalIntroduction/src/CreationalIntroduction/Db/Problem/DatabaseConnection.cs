namespace dev.kaldiroglu.CreationalIntroduction.Db.Problem;

/// <summary>
/// A connection with 15 settable properties. Nothing requires the database to be set, so a
/// forgotten assignment goes unnoticed.
/// </summary>
public class DatabaseConnection
{
    public string? Host { get; set; }
    public int Port { get; set; }
    public string? Database { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool UseSSL { get; set; }
    public int ConnectionTimeout { get; set; }
    public int ReadTimeout { get; set; }
    public string? Charset { get; set; }
    public bool AutoReconnect { get; set; }
    public int MaxRetries { get; set; }
    public string? ConnectionPool { get; set; }
    public bool EnableLogging { get; set; }
    public string? LogLevel { get; set; }
    public string? Timezone { get; set; }

    // Default constructor for post-construction initialization
    public DatabaseConnection()
    {
        // Set reasonable defaults
        Port = 3306;
        UseSSL = true;
        ConnectionTimeout = 30000;
        ReadTimeout = 60000;
        Charset = "UTF-8";
        AutoReconnect = true;
        MaxRetries = 3;
        EnableLogging = false;
        LogLevel = "INFO";
    }

    // Imagine if we tried to use constructors for all combinations:

    // Constructor for basic connection
    public DatabaseConnection(string host, string database, string username, string password)
        : this()
    {
        Host = host;
        Database = database;
        Username = username;
        Password = password;
    }

    // Constructor with port
    public DatabaseConnection(string host, int port, string database, string username, string password)
        : this(host, database, username, password)
    {
        Port = port;
    }

    // Constructor with SSL option
    public DatabaseConnection(string host, int port, string database, string username, string password, bool useSSL)
        : this(host, port, database, username, password)
    {
        UseSSL = useSSL;
    }

    // Constructor with timeouts... this quickly becomes unmanageable!
    public DatabaseConnection(string host, int port, string database, string username, string password,
                              bool useSSL, int connectionTimeout, int readTimeout)
        : this(host, port, database, username, password, useSSL)
    {
        ConnectionTimeout = connectionTimeout;
        ReadTimeout = readTimeout;
    }

    /// <summary>
    /// A database that was never set prints as the word <c>null</c>, and the flag prints in
    /// lower case.
    /// </summary>
    public override string ToString() =>
        (Host ?? "null") + ":" + Port + "/" + (Database ?? "null") + " user=" + (Username ?? "null")
        + " ssl=" + (UseSSL ? "true" : "false");
}
