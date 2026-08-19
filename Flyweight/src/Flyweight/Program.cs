using dev.kaldiroglu.Flyweight;

// Client demo (GoF, p. 204).
//
// Builds a small two-line document out of shared Character flyweights, assigns
// fonts per position range via a GlyphContext (extrinsic state), renders it,
// and reports how many character objects were saved by sharing.

var factory = new GlyphFactory();

string[] lines = { "flyweight is a nice solution", "lightweight is also a nice solution" };

// Build the document tree: a Column of Rows of (shared) Characters.
Column document = factory.CreateColumn();
int occurrences = 0;
foreach (string line in lines)
{
    Row row = factory.CreateRow();
    foreach (char c in line)
    {
        row.Insert(factory.CreateCharacter(c)); // <- sharing happens here
        occurrences++;
    }
    document.Insert(row);
}

// Extrinsic state: line 1 in Times, line 2 in Courier.
var context = new GlyphContext(new Font("Helvetica"));
context.Reset();
context.SetFont(new Font("Times"), lines[0].Length);
context.Next(lines[0].Length);
context.SetFont(new Font("Courier"), lines[1].Length);

// Render the document.
var window = new Window();
context.Reset();
document.Draw(window, context);

Console.WriteLine("Rendered document:");
Console.WriteLine(window.Text());
Console.WriteLine();
Console.WriteLine("Per-glyph rendering (char @ position : font):");
foreach (Window.RenderedGlyph r in window.Rendered)
{
    Console.WriteLine($"  '{r.Charcode}' @ ({r.X},{r.Y}) : {r.Font}");
}
Console.WriteLine();
Console.WriteLine($"Character occurrences in document : {occurrences}");
Console.WriteLine($"Distinct Character flyweights     : {factory.CreatedCharacterCount}");
Console.WriteLine($"Objects saved by sharing          : {occurrences - factory.CreatedCharacterCount}");
