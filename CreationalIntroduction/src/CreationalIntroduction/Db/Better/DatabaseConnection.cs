namespace dev.kaldiroglu.CreationalIntroduction.Db.Better;

/// <summary>
/// The same connection, which refuses to be created without its required values. It has no
/// public constructor and no setters; the only way to make one is <see cref="CreateBuilder"/>.
/// </summary>
public class DatabaseConnection
{
    public string? Host { get; }
    public int Port { get; }
    public string? Database { get; }
    public string? Username { get; }
    public string? Password { get; }
    public bool UseSSL { get; }
    public int ConnectionTimeout { get; }
    public int ReadTimeout { get; }
    public string? Charset { get; }
    public bool AutoReconnect { get; }
    public int MaxRetries { get; }
    public string? ConnectionPool { get; }
    public bool EnableLogging { get; }
    public string? LogLevel { get; }
    public string? Timezone { get; }

    private DatabaseConnection(Builder b)
    {
        Host = b.host;
        Port = b.port;
        Database = b.database;
        Username = b.username;
        Password = b.password;
        UseSSL = b.useSSL;
        ConnectionTimeout = b.connectionTimeout;
        ReadTimeout = b.readTimeout;
        Charset = b.charset;
        AutoReconnect = b.autoReconnect;
        MaxRetries = b.maxRetries;
        ConnectionPool = b.connectionPool;
        EnableLogging = b.enableLogging;
        LogLevel = b.logLevel;
        Timezone = b.timezone;
    }

    public static Builder CreateBuilder(string? host, string? database, string? username, string? password) =>
        new Builder(host, database, username, password);

    public sealed class Builder
    {
        // The members below are internal, not private: an enclosing class cannot read the
        // private members of its nested class.

        // required
        internal readonly string? host;
        internal readonly string? database;
        internal readonly string? username;
        internal readonly string? password;

        // optional with defaults
        internal int port = 3306;
        internal bool useSSL = true;
        internal int connectionTimeout = 30000;
        internal int readTimeout = 60000;
        internal string? charset = "UTF-8";
        internal bool autoReconnect = true;
        internal int maxRetries = 3;
        internal string? connectionPool;
        internal bool enableLogging = false;
        internal string? logLevel = "INFO";
        internal string? timezone;

        internal Builder(string? host, string? database, string? username, string? password)
        {
            this.host = host;
            this.database = database;
            this.username = username;
            this.password = password;
        }

        public Builder Port(int port)
        {
            this.port = port;
            return this;
        }

        public Builder UseSSL(bool useSSL)
        {
            this.useSSL = useSSL;
            return this;
        }

        public Builder ConnectionTimeout(int connectionTimeout)
        {
            this.connectionTimeout = connectionTimeout;
            return this;
        }

        public Builder ReadTimeout(int readTimeout)
        {
            this.readTimeout = readTimeout;
            return this;
        }

        public Builder Charset(string? charset)
        {
            this.charset = charset;
            return this;
        }

        public Builder AutoReconnect(bool autoReconnect)
        {
            this.autoReconnect = autoReconnect;
            return this;
        }

        public Builder MaxRetries(int maxRetries)
        {
            this.maxRetries = maxRetries;
            return this;
        }

        public Builder ConnectionPool(string? connectionPool)
        {
            this.connectionPool = connectionPool;
            return this;
        }

        public Builder EnableLogging(bool enableLogging)
        {
            this.enableLogging = enableLogging;
            return this;
        }

        public Builder LogLevel(string? logLevel)
        {
            this.logLevel = logLevel;
            return this;
        }

        public Builder Timezone(string? timezone)
        {
            this.timezone = timezone;
            return this;
        }

        public DatabaseConnection Build()
        {
            if (IsBlank(host)) throw new ArgumentException("host must not be blank");
            if (IsBlank(database)) throw new ArgumentException("database must not be blank");
            if (IsBlank(username)) throw new ArgumentException("username must not be blank");
            if (password == null) throw new ArgumentException("password must not be null");

            if (port <= 0) throw new ArgumentException("port must be > 0");
            if (connectionTimeout < 0) throw new ArgumentException("connectionTimeout must be >= 0");
            if (readTimeout < 0) throw new ArgumentException("readTimeout must be >= 0");
            if (maxRetries < 0) throw new ArgumentException("maxRetries must be >= 0");

            return new DatabaseConnection(this);
        }

        private static bool IsBlank(string? s) => s == null || s.Trim().Length == 0;
    }

    /// <summary>The flag prints in lower case.</summary>
    public override string ToString() =>
        Host + ":" + Port + "/" + Database + " user=" + Username + " ssl=" + (UseSSL ? "true" : "false");
}
