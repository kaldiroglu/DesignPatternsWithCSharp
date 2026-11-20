using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.State.Tests.Door;

using Pattern1AbstractState = global::dev.kaldiroglu.State.Door.Pattern1.AbstractDoorState;
using Pattern1ClosedDoor = global::dev.kaldiroglu.State.Door.Pattern1.ClosedDoor;
using Pattern1Door = global::dev.kaldiroglu.State.Door.Pattern1.Door;
using Pattern1OpenDoor = global::dev.kaldiroglu.State.Door.Pattern1.OpenDoor;
using Pattern1State = global::dev.kaldiroglu.State.Door.Pattern1.IDoorState;
using Pattern1Test = global::dev.kaldiroglu.State.Door.Pattern1.Main;
using Pattern2ClosedDoor = global::dev.kaldiroglu.State.Door.Pattern2.ClosedDoor;
using Pattern2Door = global::dev.kaldiroglu.State.Door.Pattern2.Door;
using Pattern2Manager = global::dev.kaldiroglu.State.Door.Pattern2.DoorStateManager;
using Pattern2OpenDoor = global::dev.kaldiroglu.State.Door.Pattern2.OpenDoor;
using Pattern2State = global::dev.kaldiroglu.State.Door.Pattern2.IDoorState;
using Pattern2Test = global::dev.kaldiroglu.State.Door.Pattern2.Main;
using ProblemDoor = global::dev.kaldiroglu.State.Door.Problem.Door;
using ProblemTest = global::dev.kaldiroglu.State.Door.Problem.Main;
using Request = global::dev.kaldiroglu.State.Door.Pattern2.Request;

/// <summary>
/// The door: no pattern (Problem), the states decide the next state (Pattern1), or a manager
/// decides it from the state and the request (Pattern2). The Part 3 slides quote these checks.
/// </summary>
public class DoorTests
{
    private const BindingFlags InstanceFields =
        BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private static readonly string[] TestOutput =
    [
        "Initial: false",
        "Door is already closed.",
        "After close(): false",
        "After open(): true",
        "Door is already open.",
        "After open(): true",
        "After close(): false",
        "After open(): true",
        "After close(): false",
        "Door is already closed.",
        "After close(): false",
    ];

    private static string[] MemberNames(Type type) =>
        type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => m is MethodInfo { IsSpecialName: false } || m is PropertyInfo)
            .Select(m => m.Name).Order().ToArray();

    [Fact]
    public void AllVersionsBehaveTheSame()
    {
        Assert.Equal(TestOutput, Printed.By(ProblemTest.Run));
        Assert.Equal(TestOutput, Printed.By(Pattern1Test.Run));
        Assert.Equal(TestOutput, Printed.By(Pattern2Test.Run));
    }

    [Fact]
    public void TheStatesDecide()
    {
        var door = new Pattern1Door();
        Assert.False(door.IsOpen);
        door.Open();
        Assert.True(door.IsOpen);
        Assert.Equal(["Door is already open."], Printed.By(door.Open));
        door.Close();
        Assert.False(door.IsOpen);

        // A Pattern1 state holds the other states.
        var fieldTypes = typeof(Pattern1AbstractState).GetFields(InstanceFields).Select(f => f.FieldType).ToList();
        Assert.Contains(typeof(Pattern1State), fieldTypes);
    }

    [Fact]
    public void Pattern1StatesAreClean()
    {
        Assert.Equal(["Close", "IsOpen", "Open"], MemberNames(typeof(Pattern1State)));
        foreach (var state in new[] { typeof(Pattern1AbstractState), typeof(Pattern1OpenDoor), typeof(Pattern1ClosedDoor) })
        {
            Assert.DoesNotContain(state.GetFields(InstanceFields), f => f.FieldType == typeof(bool));
        }
    }

    [Fact]
    public void TwoStateObjectsPerDoor()
    {
        var stateFields = typeof(Pattern1Door).GetFields(InstanceFields)
            .Where(f => typeof(Pattern1State).IsAssignableFrom(f.FieldType))
            .Count(f => f.Name != "state");
        Assert.Equal(2, stateFields);
    }

    [Fact]
    public void TheManagerDecides()
    {
        var manager = new Pattern2Manager();
        var closed = manager.Initial();
        var open = manager.Next(closed, Request.Open);
        Assert.True(open.IsOpen);
        Assert.Same(closed, manager.Next(open, Request.Close));
        // A request with no line keeps the state.
        Assert.Same(closed, manager.Next(closed, Request.Close));
        Assert.Same(open, manager.Next(open, Request.Open));
    }

    [Fact]
    public void Pattern2StatesHoldNothing()
    {
        foreach (var state in new[] { typeof(Pattern2OpenDoor), typeof(Pattern2ClosedDoor) })
        {
            Assert.Empty(state.GetFields(InstanceFields));
        }
        Assert.Equal(["Close", "IsOpen", "Open"], MemberNames(typeof(Pattern2State)));
    }

    [Fact]
    public void TheDoorAsksTheManager()
    {
        var door = new Pattern2Door();
        Assert.False(door.IsOpen);
        Assert.Equal(["Door is already closed."], Printed.By(door.Close));
        Assert.False(door.IsOpen);
        door.Open();
        Assert.True(door.IsOpen);
        Assert.Equal(["Door is already open."], Printed.By(door.Open));
        door.Close();
        Assert.False(door.IsOpen);
    }

    [Fact]
    public void TheProblemVersion()
    {
        // One boolean, two states, one if in each method.
        var code = Printed.CodeOf("Door/Problem/Door.cs");
        Assert.Equal(2, Printed.CountOf(code, "if ("));
        Assert.Single(typeof(ProblemDoor).GetFields(InstanceFields), f => f.FieldType == typeof(bool));

        var door = new ProblemDoor(false);
        Assert.Equal(["Door is already closed."], Printed.By(door.Close));
        door.Open();
        Assert.True(door.IsOpen);
        Assert.Equal(["Door is already open."], Printed.By(door.Open));
    }
}
