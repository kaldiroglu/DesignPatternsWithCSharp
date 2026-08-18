namespace dev.kaldiroglu.Bridge.Gof;

/// <summary>Prints two rendered windows next to each other, so the platforms can be compared.</summary>
public static class Display
{
    public static void SideBySide(string left, string right)
    {
        var l = left.Split('\n');
        var r = right.Split('\n');
        var width = l.Max(line => line.Length);

        for (var i = 0; i < Math.Max(l.Length, r.Length); i++)
        {
            var a = i < l.Length ? l[i] : "";
            var b = i < r.Length ? r[i] : "";
            Console.WriteLine(a.PadRight(width + 5) + b);
        }
    }

    public static void Heading(string title)
    {
        Console.WriteLine(new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    public static void Section(string title) =>
        Console.WriteLine($"\n--- {title} " + new string('-', Math.Max(0, 68 - title.Length)));
}
