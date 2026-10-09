namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;

/// <summary>
/// Sends three requests through a logging, an admin check and a header link. Every request
/// is logged; the admin check stops Elif at the admin page.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- hw-middleware</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        List<string> log = [];
        Endpoint app = MiddlewareChain.Chain(
                [Links.Logging(log), Links.AdminOnly(), Links.PoweredBy()],
                request => "200 " + request.Path);

        Console.WriteLine("elif  /home:        " + app(new HttpRequest("/home", "elif")));
        Console.WriteLine("elif  /admin/users: " + app(new HttpRequest("/admin/users", "elif")));
        Console.WriteLine("admin /admin/users: " + app(new HttpRequest("/admin/users", "admin")));
        Console.WriteLine("The log: " + Show(log));
    }

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";
}
