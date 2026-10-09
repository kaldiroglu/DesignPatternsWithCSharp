using System.Net.WebSockets;
using Xunit;

namespace dev.kaldiroglu.State.Tests;

/// <summary>
/// The .NET rows of the State deck's known-uses table, checked against the running runtime.
/// The Java <c>KnownUsesTest</c> checks the JDK rows (<c>Thread.State</c>, <c>Future.State</c>
/// and <c>FutureTask</c>); those types do not exist in .NET, so this class checks the two .NET
/// rows of the same table instead.
/// </summary>
public class KnownUsesTests
{
    [Fact]
    public void TaskStatus()
    {
        Assert.True(typeof(System.Threading.Tasks.TaskStatus).IsEnum);
        var names = Enum.GetNames<System.Threading.Tasks.TaskStatus>();
        Assert.Contains("Created", names);
        Assert.Contains("Running", names);
        Assert.Equal("Created", names[0]);
    }

    [Fact]
    public void WebSocketState()
    {
        Assert.True(typeof(WebSocketState).IsEnum);
        var names = Enum.GetNames<WebSocketState>();
        Assert.Contains("Connecting", names);
        Assert.Contains("Open", names);
    }
}
