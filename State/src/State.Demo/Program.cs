using dev.kaldiroglu.State.Hw.Vending;
// Several examples have a class called Main or Test, as the Java original does, and several
// namespaces share a name with their main class (Order, Door, Account, Elevator, Person).
// Aliases name them apart.
using OrderMain = dev.kaldiroglu.State.Order.Solution.Main;
using GofMain = dev.kaldiroglu.State.Gof.Main;
using DoorProblemTest = dev.kaldiroglu.State.Door.Problem.Test;
using DoorPattern1Test = dev.kaldiroglu.State.Door.Pattern1.Test;
using DoorPattern2Test = dev.kaldiroglu.State.Door.Pattern2.Test;
using AccountTest = dev.kaldiroglu.State.Account.Test;
using ElevatorTest = dev.kaldiroglu.State.Elevator.Test;
using PersonTest = dev.kaldiroglu.State.Person.Test;
using AirConditioner = dev.kaldiroglu.State.Hw.AirConditioner.AirConditioner;
// The homework's Action enum shares its name with System.Action, which the table below uses.
using DocumentAction = dev.kaldiroglu.State.Hw.Document.Action;
using Workflow = dev.kaldiroglu.State.Hw.Document.Workflow;
using WorkflowDocument = dev.kaldiroglu.State.Hw.Document.Document;

namespace dev.kaldiroglu.State.Demo;

/// <summary>
/// Runs the State examples.
/// <para>
/// <c>order</c>, <c>gof</c>, <c>door-problem</c>, <c>door-pattern1</c>, <c>door-pattern2</c>,
/// <c>account</c>, <c>elevator</c> and <c>person</c> are the Java original's <c>main</c>
/// methods and print the same output. The three homework exercises have no <c>main</c> in
/// Java and are only in this runner. The <c>Pattern</c> outline has empty methods, so there
/// is nothing to run.
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
        ["order"] = ("AN ONLINE ORDER", OrderMain.Run),
        ["gof"] = ("GOF'S TCP CONNECTION", GofMain.Run),
        ["door-problem"] = ("A DOOR", DoorProblemTest.Run),
        ["door-pattern1"] = ("A DOOR", DoorPattern1Test.Run),
        ["door-pattern2"] = ("A DOOR", DoorPattern2Test.Run),
        ["account"] = ("A BANK ACCOUNT", AccountTest.Run),
        ["elevator"] = ("AN ELEVATOR", ElevatorTest.Run),
        ["person"] = ("A PERSON", PersonTest.Run),
        ["hw-airconditioner"] = ("HOMEWORK", AirConditionerHomework),
        ["hw-document"] = ("HOMEWORK", DocumentHomework),
        ["hw-vending"] = ("HOMEWORK", VendingHomework)
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

    // ------------------------------------------------------------ homework

    /// <summary>
    /// The room is 28. The target is set to 24 while the unit is off, so it stays off. Then it
    /// is turned on, the room cools to 24 and then to 20, and it is turned off.
    /// </summary>
    private static void AirConditionerHomework()
    {
        var ac = new AirConditioner(28);
        ac.SetTarget(24);
        Console.WriteLine("Target 24 while off: " + ac.State);
        ac.PowerOn();
        Console.WriteLine("Power on:            " + ac.State);
        ac.RoomIs(24);
        Console.WriteLine("Room is 24:          " + ac.State);
        ac.RoomIs(20);
        Console.WriteLine("Room is 20:          " + ac.State);
        ac.PowerOff();
        Console.WriteLine("Power off:           " + ac.State);
        Console.WriteLine("Log: " + Show(ac.Log));
    }

    /// <summary>A document is submitted, rejected, submitted again and approved. Then it is submitted once more.</summary>
    private static void DocumentHomework()
    {
        var document = new WorkflowDocument(new Workflow());
        foreach (var action in new[]
                 {
                     DocumentAction.SUBMIT, DocumentAction.REJECT, DocumentAction.SUBMIT,
                     DocumentAction.APPROVE, DocumentAction.SUBMIT
                 })
        {
            try
            {
                document.Apply(action);
                Console.WriteLine(action + " -> " + document.Status);
            }
            catch (InvalidOperationException refused)
            {
                Console.WriteLine(action + ": " + refused.Message);
            }
        }
    }

    /// <summary>A machine with one drink: a coin is needed, one coin is enough, and a sold-out machine returns the coin.</summary>
    private static void VendingHomework()
    {
        var machine = new VendingMachine(1);
        machine.PressButton();
        machine.InsertCoin();
        machine.InsertCoin();
        machine.PressButton();
        machine.InsertCoin();
        Console.WriteLine("After one drink: " + machine.State);
        machine.Refill(2);
        Console.WriteLine("After refill 2:  " + machine.State);
        machine.InsertCoin();
        machine.PressButton();
        Console.WriteLine("At the end:      " + machine.State);
        Console.WriteLine("Log: " + Show(machine.Log));
    }

    // ---------------------------------------------------------------- output

    /// <summary>Prints a list the way Java's <c>List.toString()</c> does: <c>[a, b, c]</c>.</summary>
    private static string Show(IEnumerable<string> items) => "[" + string.Join(", ", items) + "]";

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
