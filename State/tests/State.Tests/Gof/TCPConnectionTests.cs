using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.State.Tests.Gof;

using dev.kaldiroglu.State.Gof.Solution;
using GofMain = global::dev.kaldiroglu.State.Gof.Main;
using SwitchingConnection = global::dev.kaldiroglu.State.Gof.Problem.TCPConnection;

/// <summary>
/// GoF's TCP connection (Design Patterns, pp. 305-313), before and after the pattern. The
/// Part 2 slides say the two versions give identical logs; this class checks it. Ported from
/// the Java <c>TCPConnectionTest</c>.
/// </summary>
public class TCPConnectionTests
{
    private const string Problem = "Gof/Problem/TCPConnection.cs";

    private static readonly string[] Requests = ["ActiveOpen", "PassiveOpen", "Send", "Acknowledge", "Close"];

    private sealed class Pair
    {
        public SwitchingConnection Before { get; } = new();
        public TCPConnection After { get; } = new();

        public void Run(string request)
        {
            switch (request)
            {
                case "ActiveOpen": Before.ActiveOpen(); After.ActiveOpen(); break;
                case "PassiveOpen": Before.PassiveOpen(); After.PassiveOpen(); break;
                case "Send": Before.Send("data"); After.Send("data"); break;
                case "Acknowledge": Before.Acknowledge(); After.Acknowledge(); break;
                case "Close": Before.Close(); After.Close(); break;
                default: throw new ArgumentException(request);
            }
        }
    }

    [Fact]
    public void BothVersionsAgree()
    {
        foreach (var first in Requests)
        {
            foreach (var second in Requests)
            {
                foreach (var third in Requests)
                {
                    var pair = new Pair();
                    pair.Run(first);
                    pair.Run(second);
                    pair.Run(third);
                    var path = first + ", " + second + ", " + third;
                    Assert.True(pair.Before.Log.SequenceEqual(pair.After.Log), path);
                    Assert.True(pair.Before.State == pair.After.State, path);
                }
            }
        }
    }

    [Fact]
    public void Established()
    {
        var connection = new TCPConnection();
        connection.ActiveOpen();
        Assert.Equal("ESTABLISHED", connection.State);

        connection.Send("hello");
        connection.Acknowledge();
        connection.Close();

        Assert.Equal(["send SYN", "sent: hello", "ACK", "send FIN"], connection.Log);
        Assert.Equal("LISTEN", connection.State);
    }

    [Fact]
    public void ListenAndClosed()
    {
        var connection = new TCPConnection();
        connection.Send("x");
        connection.PassiveOpen();
        Assert.Equal("LISTEN", connection.State);
        connection.Send("x");

        Assert.Equal(["ignored: send", "send SYN, SYN-ACK"], connection.Log);
        Assert.Equal("ESTABLISHED", connection.State);
    }

    [Fact]
    public void MainOutput()
    {
        var lines = Printed.By(GofMain.Run);

        var log = "[ignored: acknowledge, send SYN, sent: hello, ACK, send FIN] -> LISTEN";
        Assert.Equal(["Switch:       " + log, "State objects: " + log], lines);
    }

    [Fact]
    public void FiveRequestsThreeStates()
    {
        var code = Printed.CodeOf(Problem);

        Assert.Equal(5, Printed.CountOf(code, "switch (state)"));
        var states = typeof(SwitchingConnection).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public)
            .Single(t => t.IsEnum);
        Assert.Equal(3, Enum.GetValues(states).Length);

        // The rules of the established state are in all five methods.
        var established = code.Split("public void ")
            .Skip(1)
            .Take(5)
            .Count(method => method.Contains("ESTABLISHED"));
        Assert.Equal(5, established);
    }

    [Fact]
    public void DefaultsAndChangeState()
    {
        Assert.True(typeof(TCPState).IsAbstract);
        foreach (var request in Requests)
        {
            var method = typeof(TCPState).GetMethod(request,
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
            Assert.NotNull(method);
            Assert.False(method.IsAbstract, request);
        }

        // Java's changeState is package-private. The C# port makes it internal: not public,
        // not protected, not private.
        var changeState = typeof(TCPConnection).GetMethod("ChangeState",
            BindingFlags.Instance | BindingFlags.NonPublic, [typeof(TCPState)])!;
        Assert.NotNull(changeState);
        Assert.True(changeState.IsAssembly, "ChangeState is internal");
    }

    [Fact]
    public void SharedStates()
    {
        foreach (var state in new[] { typeof(TCPClosed), typeof(TCPListen), typeof(TCPEstablished) })
        {
            var fields = state.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance
                                         | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.True(fields.All(f => f.IsStatic), state.Name);
            var constructors = state.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.True(constructors.All(c => c.IsPrivate), state.Name);
        }
        Assert.Equal("CLOSED", new TCPConnection().State);
    }
}
