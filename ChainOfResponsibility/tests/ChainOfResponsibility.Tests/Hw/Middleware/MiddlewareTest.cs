namespace dev.kaldiroglu.ChainOfResponsibility.Tests.Hw.Middleware;

using dev.kaldiroglu.ChainOfResponsibility.Hw.Middleware;
using Xunit;

/// <summary>Homework 2: a middleware chain, where every link may act and most pass the request on.</summary>
public class MiddlewareTest
{
    /// <summary>Every link runs for a normal page, in the order of the list.</summary>
    [Fact]
    public void EveryLinkRuns()
    {
        List<string> log = [];
        Endpoint server = MiddlewareChain.Chain(
            [Links.Logging(log), Links.AdminOnly(), Links.PoweredBy()],
            request => "200 " + request.Path);

        Assert.Equal("200 /home [powered by the chain]", server(new HttpRequest("/home", "elif")));
        Assert.Equal(new[] { "elif /home" }, log);
    }

    /// <summary>The admin check answers 403 itself, so the links after it never run.</summary>
    [Fact]
    public void TheAdminCheckStopsTheRequest()
    {
        List<string> log = [];
        List<string> reached = [];
        Endpoint server = MiddlewareChain.Chain(
            [Links.Logging(log), Links.AdminOnly(), Links.PoweredBy()],
            request =>
            {
                reached.Add(request.Path);
                return "200 " + request.Path;
            });

        Assert.Equal("403 forbidden", server(new HttpRequest("/admin/users", "elif")));
        // The logging link before it still ran.
        Assert.Equal(new[] { "elif /admin/users" }, log);
        Assert.Empty(reached);
        Assert.Equal("200 /admin/users [powered by the chain]",
            server(new HttpRequest("/admin/users", "admin")));
    }

    /// <summary>The list decides the order: with the admin check first, a refused request is not logged.</summary>
    [Fact]
    public void TheListDecidesTheOrder()
    {
        List<string> log = [];
        Endpoint server = MiddlewareChain.Chain(
            [Links.AdminOnly(), Links.Logging(log)],
            request => "200 " + request.Path);

        Assert.Equal("403 forbidden", server(new HttpRequest("/admin", "elif")));
        Assert.Empty(log);
    }
}
