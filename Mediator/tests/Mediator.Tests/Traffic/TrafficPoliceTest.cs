namespace dev.kaldiroglu.Mediator.Tests.Traffic;

using dev.kaldiroglu.Mediator.Traffic;
using Xunit;
using static dev.kaldiroglu.Mediator.Tests.Printed;

/// <summary>
/// The traffic police officer. Most tests run on one thread, with a small vehicle that only
/// records what it was told. The last two run real threads.
/// </summary>
public class TrafficPoliceTest
{
    /// <summary>A vehicle that records what the officer told it.</summary>
    private sealed class Recorder(string name, List<string> told) : IVehicle
    {
        public void Approach() => told.Add(name + " approaches");

        public void Proceed() => told.Add(name + " proceeds");

        public void Stop() => told.Add(name + " stops");
    }

    /// <summary>A vehicle that blocks its thread until the officer tells it to proceed.</summary>
    private sealed class Blocking : IVehicle
    {
        private readonly ManualResetEventSlim permit = new(false);

        public void Approach()
        {
        }

        public void Proceed() => permit.Set();

        public void Stop()
        {
        }

        public void WaitForPermit() => permit.Wait();
    }

    private static Junction NewJunction()
    {
        Junction? junction = null;
        By(() => junction = new Junction("Main Square"));
        return junction!;
    }

    private static TrafficPolice OfficerOf(Junction junction)
    {
        TrafficPolice? officer = null;
        By(() => officer = new TrafficPolice("Ali", junction));
        return officer!;
    }

    /// <summary>The officer tells the first waiting car to proceed, and that car does not ask again.</summary>
    [Fact]
    public void TheOfficerTellsTheNextCar()
    {
        List<string> told = [];
        TrafficPolice officer = OfficerOf(NewJunction());
        IVehicle first = new Recorder("first", told);
        IVehicle second = new Recorder("second", told);

        officer.AskPermitToPass(first);
        officer.AskPermitToPass(second);
        officer.Done(first);

        Assert.Equal(new[] { "first proceeds", "second stops", "second proceeds" }, told);
    }

    /// <summary>Cars pass in the order they asked.</summary>
    [Fact]
    public void WaitingCarsPassInOrder()
    {
        List<string> told = [];
        TrafficPolice officer = OfficerOf(NewJunction());
        IVehicle first = new Recorder("first", told);
        IVehicle second = new Recorder("second", told);
        IVehicle third = new Recorder("third", told);

        officer.AskPermitToPass(first);
        officer.AskPermitToPass(second);
        officer.AskPermitToPass(third);
        officer.Done(first);
        officer.Done(second);

        Assert.Equal(new[]
        {
            "first proceeds",
            "second stops",
            "third stops",
            "second proceeds",
            "third proceeds"
        }, told);
    }

    /// <summary>The junction stays busy from one car to the next, and is free after the last.</summary>
    [Fact]
    public void TheJunctionStaysBusyUntilTheLastCar()
    {
        List<string> told = [];
        Junction junction = NewJunction();
        TrafficPolice officer = OfficerOf(junction);
        IVehicle first = new Recorder("first", told);
        IVehicle second = new Recorder("second", told);
        IVehicle late = new Recorder("late", told);

        officer.AskPermitToPass(first);
        officer.AskPermitToPass(second);
        officer.Done(first);
        Assert.True(junction.IsBusy());

        officer.AskPermitToPass(late);
        Assert.Equal("late stops", told[^1]);

        officer.Done(second);
        officer.Done(late);
        Assert.False(junction.IsBusy());
    }

    /// <summary>200 threads: every one passes, and never two in the junction at once.</summary>
    [Fact]
    public void OneAtATimeUnderThreads()
    {
        TrafficPolice officer = OfficerOf(NewJunction());
        int inJunction = 0;
        int mostAtOnce = 0;
        int passed = 0;
        object counters = new();

        Thread[] threads = new Thread[200];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                Blocking vehicle = new();
                officer.AskPermitToPass(vehicle);
                vehicle.WaitForPermit();
                lock (counters)
                {
                    inJunction++;
                    mostAtOnce = Math.Max(mostAtOnce, inJunction);
                    passed++;
                    inJunction--;
                }
                officer.Done(vehicle);
            });
        }
        foreach (Thread thread in threads)
        {
            thread.Start();
        }
        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Assert.Equal(200, passed);
        Assert.Equal(1, mostAtOnce);
    }

    /// <summary>Main: five cars, each asks once and each passes once.</summary>
    [Fact]
    public void MainOutput()
    {
        IReadOnlyList<string> lines = By(Main.Run);

        Assert.Equal(5, lines.Count(line => line.EndsWith("is asking permit to pass the junction.")));
        Assert.Equal(5, lines.Count(line => line.EndsWith("is proceeding through the junction.")));
    }

    /// <summary>A colleague knows only the mediator: Car holds no Junction and has no loop.</summary>
    [Fact]
    public void ACarKnowsOnlyTheMediator()
    {
        string car = CodeOf("Traffic/Car.cs");

        Assert.DoesNotContain("Junction ", car);
        Assert.DoesNotContain("while", car);
    }
}
