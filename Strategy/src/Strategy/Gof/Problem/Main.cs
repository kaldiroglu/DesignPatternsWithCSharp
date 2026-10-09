namespace dev.kaldiroglu.Strategy.Gof.Problem;

/// <summary>Breaks one paragraph with both algorithms that live inside the composition, chosen by a flag.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- gof-problem</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        const string text = "A document editor breaks a stream of text into lines "
            + "and there are many algorithms for it";
        var words = new List<Component>();
        foreach (var word in text.Split(' '))
        {
            words.Add(Component.Word(word));
        }

        foreach (var quality in new[] { false, true })
        {
            var layout = new Composition(words, 26, quality).Repair();
            Console.WriteLine("quality = " + (quality ? "true" : "false") + ", worst slack " + layout.WorstSlack + ":");
            foreach (var line in layout.Render()) Console.WriteLine("  |" + line);
        }
        Console.WriteLine("Both algorithms are private methods of the composition.");
        Console.WriteLine("A boolean set in the constructor chooses one.");
        Console.WriteLine("A third algorithm does not fit in a boolean.");
    }
}
