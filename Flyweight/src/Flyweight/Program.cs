using dev.kaldiroglu.Flyweight;

// Client demo (GoF, p. 204).
//
// With no argument, it assigns fonts per position range via a GlyphContext (extrinsic state),
// renders the document, and reports how many character objects were saved by sharing. One
// entry runs on its own - `dotnet run -- forest-problem` - and prints one slide's output.

var examples = new (string Name, Action Run)[]
{
    ("gof-font-ranges", FontRanges),
    ("gof-sharing-count", SharingCount),
    ("forest-problem", dev.kaldiroglu.Flyweight.Forest.Problem.ForestDemo.Run),
    ("forest", dev.kaldiroglu.Flyweight.Forest.Solution.ForestDemo.Run),
    ("book-wrong", dev.kaldiroglu.Flyweight.Book.Wrong.BookDemo.Run),
    ("book-correct", dev.kaldiroglu.Flyweight.Book.Correct.BookDemo.Run),
    ("quote-problem", dev.kaldiroglu.Flyweight.Quote.Problem.QuoteDemo.Run),
};

if (args.Length > 0)
{
    var name = args[0].ToLowerInvariant();
    var example = examples.FirstOrDefault(e => e.Name == name);
    if (example.Run is null)
    {
        Console.WriteLine($"unknown example '{name}'. One of: "
                          + string.Join(", ", examples.Select(e => e.Name)));
        return;
    }

    example.Run();
    return;
}

FontRanges();
Console.WriteLine();
SharingCount();

static void FontRanges()
{
    var sample = new SampleDocument();
    string[] lines = sample.Lines;

    // Extrinsic state: line 1 in Times, line 2 in Courier.
    var context = new GlyphContext(new Font("Helvetica"));
    context.Reset();
    context.SetFont(new Font("Times"), lines[0].Length);
    context.Next(lines[0].Length);
    context.SetFont(new Font("Courier"), lines[1].Length);

    // Render the document.
    var window = new Window();
    context.Reset();
    sample.Document.Draw(window, context);

    Console.WriteLine("Rendered document:");
    Console.WriteLine(window.Text());
    Console.WriteLine();
    Console.WriteLine("Per-glyph rendering (char @ position : font):");
    foreach (Window.RenderedGlyph r in window.Rendered)
    {
        Console.WriteLine($"  '{r.Charcode}' @ ({r.X},{r.Y}) : {r.Font}");
    }
}

static void SharingCount()
{
    var sample = new SampleDocument();

    Console.WriteLine($"Character occurrences in document : {sample.Occurrences}");
    Console.WriteLine($"Distinct Character flyweights     : {sample.Factory.CreatedCharacterCount}");
    Console.WriteLine($"Objects saved by sharing          : {sample.Occurrences - sample.Factory.CreatedCharacterCount}");
}
