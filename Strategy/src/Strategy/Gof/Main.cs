namespace dev.kaldiroglu.Strategy.Gof;

/// <summary>
/// Shows the shared types on their own: words as components, and a layout that measures its
/// lines.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- gof-layout</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        IReadOnlyList<Component> first = [Component.Word("A"), Component.Word("document"), Component.Word("editor")];
        IReadOnlyList<Component> second = [Component.Word("breaks"), Component.Word("lines")];
        var layout = new Layout([first, second], 20);

        Console.WriteLine("A layout of " + layout.LineCount + " lines in a 20-column measure:");
        for (var i = 0; i < layout.LineCount; i++)
        {
            Console.WriteLine("  '" + layout.Render()[i] + "' width " + layout.WidthOf(i)
                + ", slack " + layout.SlackOn(i));
        }
        Console.WriteLine("Worst slack, last line not counted: " + layout.WorstSlack);
    }
}
