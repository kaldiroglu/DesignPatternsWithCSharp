using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.State.Tests.Door;

using Pattern1Door = global::dev.kaldiroglu.State.Door.Pattern1.Door;
using Pattern1State = global::dev.kaldiroglu.State.Door.Pattern1.IDoorState;
using Pattern1Test = global::dev.kaldiroglu.State.Door.Pattern1.Test;
using Pattern2AbstractDoor = global::dev.kaldiroglu.State.Door.Pattern2.AbstractDoor;
using Pattern2Door = global::dev.kaldiroglu.State.Door.Pattern2.Door;
using Pattern2Manager = global::dev.kaldiroglu.State.Door.Pattern2.DoorStateManager;
using Pattern2State = global::dev.kaldiroglu.State.Door.Pattern2.IDoorState;
using Pattern2Test = global::dev.kaldiroglu.State.Door.Pattern2.Test;
using ProblemDoor = global::dev.kaldiroglu.State.Door.Problem.Door;

/// <summary>
/// The door: the states decide the next state (Pattern1), or a manager decides (Pattern2).
/// The Part 3 slides say both versions behave the same; this class checks it. Ported from
/// the Java <c>DoorTest</c>.
/// </summary>
public class DoorTests
{
    private const BindingFlags InstanceFields =
        BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private static readonly string[] TestOutput =
    [
        "Initial: false",
        "Door is already closed!",
        "After close(): false",
        "After open(): true",
        "Door is already open!",
        "After open(): true",
        "After close(): false",
        "After open(): true",
        "After close(): false",
        "Door is already closed!",
        "After close(): false",
    ];

    [Fact]
    public void BothVersionsBehaveTheSame()
    {
        var first = Printed.By(Pattern1Test.Run);
        var second = Printed.By(Pattern2Test.Run);

        Assert.Equal(TestOutput, first);
        Assert.Equal(TestOutput.Select(line => line.Replace('!', '.')), second);
    }

    [Fact]
    public void TheStatesDecide()
    {
        var door = new Pattern1Door();
        Assert.False(door.IsOpen);
        door.Open();
        Assert.True(door.IsOpen);
        Assert.Equal(["Door is already open!"], Printed.By(door.Open));
        door.Close();
        Assert.False(door.IsOpen);
    }

    [Fact]
    public void TheManagerDecides()
    {
        var door = new Pattern2Door();
        Assert.False(door.IsOpen);
        door.Open();
        Assert.True(door.IsOpen);
        door.Close();
        Assert.False(door.IsOpen);

        var managerFields = typeof(Pattern2Manager).GetFields(InstanceFields).Select(f => f.Name).ToList();
        Assert.Contains("openState", managerFields);
        Assert.Contains("closedState", managerFields);

        // A Pattern2 state does not hold another state.
        var stateFieldTypes = typeof(Pattern2AbstractDoor).GetFields(InstanceFields).Select(f => f.FieldType).ToList();
        Assert.DoesNotContain(typeof(Pattern2State), stateFieldTypes);
    }

    [Fact]
    public void TwoStateObjectsPerDoor()
    {
        var stateFields = typeof(Pattern1Door).GetFields(InstanceFields)
            .Where(f => f.FieldType == typeof(Pattern1State))
            .Count(f => f.Name != "state");
        Assert.Equal(2, stateFields);
    }

    [Fact]
    public void TheProblemVersion()
    {
        var door = new ProblemDoor(false);
        Assert.Equal(["Door is already closed."], Printed.By(door.Close));
        door.Open();
        Assert.True(door.IsOpen);
        Assert.Equal(["Door is already open."], Printed.By(door.Open));
    }
}
