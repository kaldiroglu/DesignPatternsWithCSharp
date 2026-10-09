namespace dev.kaldiroglu.Mediator.Tests.Traffic;

using dev.kaldiroglu.Mediator.Traffic;
using Xunit;
using static dev.kaldiroglu.Mediator.Tests.Printed;

/// <summary>
/// The traffic police officer, on one thread and without sleeping. Car sleeps for a second when
/// it waits, so this test uses its own small vehicle that only records what it was told.
/// </summary>
public class TrafficPoliceTest
{
    /// <summary>A vehicle that records what the officer told it, and leaves the junction only when asked.</summary>
    private sealed class Recorder(string name, List<string> told) : IVehicle
    {
        public void Approach() => told.Add(name + " approaches");

        public void Proceed() => told.Add(name + " proceeds");

        public void Stop() => told.Add(name + " stops");

        public void WaitForAWhile() => told.Add(name + " waits");
    }

    /// <summary>One vehicle in the junction at a time: the second waits until the first is done.</summary>
    [Fact]
    public void OneAtATime()
    {
        List<string> told = [];
        Junction? junction = null;
        TrafficPolice? officer = null;
        Assert.Equal(new[] { "Junction Main Square created.", "TrafficPolice Ali created." }, By(() =>
        {
            junction = new Junction("Main Square");
            officer = new TrafficPolice("Ali", junction);
        }));
        IVehicle first = new Recorder("first", told);
        IVehicle second = new Recorder("second", told);
        officer!.Receive(first);
        officer.Receive(second);
        officer.AskPermitToPass(first);
        Assert.True(junction!.IsBusy());
        officer.AskPermitToPass(second);
        officer.Done(first);
        Assert.False(junction.IsBusy());
        officer.AskPermitToPass(second);
        Assert.Equal(new[]
        {
            "first stops",
            "second stops",
            "first proceeds",
            "second waits",
            "second proceeds"
        }, told);
    }
}
