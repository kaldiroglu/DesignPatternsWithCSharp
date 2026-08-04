namespace dev.kaldiroglu.Composite.Gof.Equipment;

/// <summary>
/// Client of the equipment Composite — the assembly code of GoF p. 173.
/// </summary>
/// <remarks>
/// The book's client is reproduced almost line for line:
/// <code>
/// Cabinet* cabinet = new Cabinet("PC Cabinet");
/// Chassis* chassis = new Chassis("PC Chassis");
/// cabinet->Add(chassis);
/// Bus* bus = new Bus("MCA Bus");
/// bus->Add(new Card("16Mbs Token Ring"));
/// chassis->Add(bus);
/// chassis->Add(new FloppyDisk("3.5in Floppy"));
/// cout &lt;&lt; "The net price is " &lt;&lt; chassis->NetPrice() &lt;&lt; endl;
/// </code>
/// </remarks>
public static class EquipmentDemo
{
    public static void Run()
    {
        var cabinet = new Cabinet("PC Cabinet");
        var chassis = new Chassis("PC Chassis");
        cabinet.Add(chassis);

        var bus = new Bus("MCA Bus");
        bus.Add(new Card("16Mbs Token Ring"));
        chassis.Add(bus);
        chassis.Add(new FloppyDisk("3.5in Floppy"));

        Console.WriteLine("--- The assembled equipment ---");
        PrintTree(cabinet, "");

        Console.WriteLine();
        Console.WriteLine("--- One call, answered by the whole subtree ---");
        Console.WriteLine($"The net price of the chassis is {chassis.NetPrice()}");
        Console.WriteLine($"The net price of the cabinet is {cabinet.NetPrice()}");
        Console.WriteLine($"The cabinet draws {cabinet.Power()} W");
        Console.WriteLine($"The cabinet's discount price is {cabinet.DiscountPrice()}");

        Console.WriteLine();
        Console.WriteLine("--- The same calls work on a single leaf ---");
        Equipment lone = new Card("Ethernet");
        Console.WriteLine($"{lone.Name}: net {lone.NetPrice()}, "
                          + $"discount {lone.DiscountPrice()}, {lone.Power()} W");
    }

    /// <summary>
    /// Walks an arbitrary equipment tree. The recursion needs no type test — a
    /// leaf simply yields an empty enumeration and the walk stops there.
    /// </summary>
    private static void PrintTree(Equipment equipment, string indent)
    {
        Console.WriteLine(
            $"{indent}{equipment.Name} (net {equipment.NetPrice()}, {equipment.Power()} W)");
        foreach (var part in equipment)
        {
            PrintTree(part, indent + "    ");
        }
    }
}
