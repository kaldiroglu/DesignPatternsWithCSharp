namespace dev.kaldiroglu.Strategy.Gof.Solution;

/// <summary>Breaks one paragraph with three compositors, changed on the same composition while it runs.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- gof-solution</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        const string text = "A document editor breaks a stream of text into lines "
            + "and there are many algorithms for it";
        var document = new Composition(26, new SimpleCompositor());
        foreach (var word in text.Split(' '))
        {
            document.Insert(Component.Word(word));
        }

        ICompositor[] compositors = [new SimpleCompositor(), new TeXCompositor(), new ArrayCompositor(6)];
        foreach (var compositor in compositors)
        {
            document.SetCompositor(compositor);
            var layout = document.Repair();
            Console.WriteLine(document.CompositorName + ", " + layout.LineCount + " lines:");
            foreach (var line in layout.Render()) Console.WriteLine("  |" + line);
        }
        Console.WriteLine("One composition, three algorithms, and the composition names none of them.");
    }
}
