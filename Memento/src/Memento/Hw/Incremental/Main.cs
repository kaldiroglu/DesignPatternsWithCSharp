using System.Globalization;

namespace dev.kaldiroglu.Memento.Hw.Incremental;

/// <summary>
/// Edits two cells of a 100 by 100 sheet and undoes the edit. The memento holds the old
/// values of the two changed cells only.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Memento.Demo -- hw-incremental</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Sheet sheet = new Sheet(100, 100);
        Sheet.IChange change = sheet.Set(new Dictionary<string, int> { ["R1C1"] = 5, ["R1C2"] = 7 });

        Console.WriteLine("Cells in the sheet: " + Number(sheet.CellCount));
        Console.WriteLine("Cells in the memento: " + Number(change.Size));
        Console.WriteLine("After the edit: R1C1 = " + Number(sheet.Get("R1C1")) + ", R1C2 = " + Number(sheet.Get("R1C2")));

        sheet.Undo(change);
        Console.WriteLine("After undo:     R1C1 = " + Number(sheet.Get("R1C1")) + ", R1C2 = " + Number(sheet.Get("R1C2")));
    }

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);
}
