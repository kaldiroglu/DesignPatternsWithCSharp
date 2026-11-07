using Xunit;

namespace dev.kaldiroglu.State.Tests.Elevator;

using Elevator = global::dev.kaldiroglu.State.Elevator.Elevator;
using IElevatorState = global::dev.kaldiroglu.State.Elevator.IElevatorState;
using StoppedState = global::dev.kaldiroglu.State.Elevator.StoppedState;
using ElevatorMain = global::dev.kaldiroglu.State.Elevator.Main;

/// <summary>
/// The elevator: the caller changes the state, and the states only print. The State deck
/// lists this example as the caller-driven one, closer to Strategy.
/// </summary>
public class ElevatorTests
{
    [Fact]
    public void MainPrintsBothTrips()
    {
        Assert.Equal(new[]
        {
            "Door is open.",
            "Starting to go up.",
            "Please first stop the elevator.",
            "Please first stop the elevator.",
            "Stopping!",
            "Door is open.",
            "Starting to go down.",
            "Already going down.",
            "Stopping!"
        }, Printed.By(ElevatorMain.Run));
    }

    [Fact]
    public void TheStatesNeverChangeTheState()
    {
        IElevatorState stopped = new StoppedState();
        var elevator = new Elevator(stopped);
        Printed.By(() =>
        {
            elevator.GoUp();
            elevator.GoDown();
            elevator.Stop();
            elevator.OpenDoor();
        });
        Assert.Same(stopped, elevator.State);
    }

    [Fact]
    public void AStoppedElevatorSaysItIsStarting()
    {
        var elevator = new Elevator(new StoppedState());
        Assert.Equal(new[] { "Starting to go up." }, Printed.By(elevator.GoUp));
    }

    [Fact]
    public void TheElevatorIsNotAState()
    {
        Assert.False(typeof(IElevatorState).IsAssignableFrom(typeof(Elevator)));
    }
}
