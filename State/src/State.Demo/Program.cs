// Several examples have a class called Main or Test, the order's
// problem and solution both have a SecondShipmentMain, and several
// namespaces share a name with their main class (Order, Door, Account, Elevator, Person).
// Aliases name them apart.
using OrderProblemStageOneMain = dev.kaldiroglu.State.Order.Problem.StageOneMain;
using OrderProblemStageTwoMain = dev.kaldiroglu.State.Order.Problem.StageTwoMain;
using OrderProblemStageThreeMain = dev.kaldiroglu.State.Order.Problem.StageThreeMain;
using OrderProblemSecondShipmentMain = dev.kaldiroglu.State.Order.Problem.SecondShipmentMain;
using OrderRefusedRequestMain = dev.kaldiroglu.State.Order.Solution.RefusedRequestMain;
using OrderSecondShipmentMain = dev.kaldiroglu.State.Order.Solution.SecondShipmentMain;
using GofMain = dev.kaldiroglu.State.Gof.Main;
using GofProblemMain = dev.kaldiroglu.State.Gof.Problem.Main;
using GofSolutionMain = dev.kaldiroglu.State.Gof.Solution.Main;
using PatternMain = dev.kaldiroglu.State.Pattern.Main;
using DoorProblemTest = dev.kaldiroglu.State.Door.Problem.Test;
using DoorPattern1Test = dev.kaldiroglu.State.Door.Pattern1.Test;
using DoorPattern2Test = dev.kaldiroglu.State.Door.Pattern2.Test;
using AccountTest = dev.kaldiroglu.State.Account.Test;
using ElevatorTest = dev.kaldiroglu.State.Elevator.Test;
using PersonTest = dev.kaldiroglu.State.Person.Test;
using AirConditionerMain = dev.kaldiroglu.State.Hw.AirConditioner.Main;
using DocumentMain = dev.kaldiroglu.State.Hw.Document.Main;
using VendingMain = dev.kaldiroglu.State.Hw.Vending.Main;

namespace dev.kaldiroglu.State.Demo;

/// <summary>
/// Runs the State examples.
/// <para>
/// The <c>Pattern</c> outline has empty methods, so <c>pattern</c> only names its classes.
/// </para>
/// <para>
/// Each example runs on its own — <c>dotnet run -- gof</c> — and with no argument all of them
/// run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["order-problem-stage-one"] = ("AN ONLINE ORDER", OrderProblemStageOneMain.Run),
        ["order-problem-stage-two"] = ("AN ONLINE ORDER", OrderProblemStageTwoMain.Run),
        ["order-problem-stage-three"] = ("AN ONLINE ORDER", OrderProblemStageThreeMain.Run),
        ["order-problem-second-shipment"] = ("AN ONLINE ORDER", OrderProblemSecondShipmentMain.Run),
        ["order-second-shipment"] = ("AN ONLINE ORDER", OrderSecondShipmentMain.Run),
        ["order-refused-request"] = ("AN ONLINE ORDER", OrderRefusedRequestMain.Run),
        ["gof-problem"] = ("GOF'S TCP CONNECTION", GofProblemMain.Run),
        ["gof-solution"] = ("GOF'S TCP CONNECTION", GofSolutionMain.Run),
        ["gof"] = ("GOF'S TCP CONNECTION", GofMain.Run),
        ["pattern"] = ("GOF'S TCP CONNECTION, SHORT FORM", PatternMain.Run),
        ["door-problem"] = ("A DOOR", DoorProblemTest.Run),
        ["door-pattern1"] = ("A DOOR", DoorPattern1Test.Run),
        ["door-pattern2"] = ("A DOOR", DoorPattern2Test.Run),
        ["account"] = ("A BANK ACCOUNT", AccountTest.Run),
        ["elevator"] = ("AN ELEVATOR", ElevatorTest.Run),
        ["person"] = ("A PERSON", PersonTest.Run),
        ["hw-airconditioner"] = ("HOMEWORK", AirConditionerMain.Run),
        ["hw-document"] = ("HOMEWORK", DocumentMain.Run),
        ["hw-vending"] = ("HOMEWORK", VendingMain.Run)
    };

    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            var name = args[0].ToLowerInvariant();
            if (!Examples.TryGetValue(name, out var example))
            {
                Console.WriteLine($"unknown example '{name}'. One of: {string.Join(", ", Examples.Keys)}");
                return;
            }

            example.Run();
            return;
        }

        string? lastGroup = null;
        foreach (var (name, (group, run)) in Examples)
        {
            if (group != lastGroup)
            {
                Heading(group);
                lastGroup = group;
            }

            Section(name);
            run();
        }
    }

    // ---------------------------------------------------------------- output

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
