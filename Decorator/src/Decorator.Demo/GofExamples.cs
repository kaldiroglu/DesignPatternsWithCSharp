using dev.kaldiroglu.Decorator.Gof.Visual.Solution;
using Stream = dev.kaldiroglu.Decorator.Gof.Stream.Stream;
using StreamProblems = dev.kaldiroglu.Decorator.Gof.Stream.Problem;
using StreamSolutions = dev.kaldiroglu.Decorator.Gof.Stream.Solution;
using VisualProblems = dev.kaldiroglu.Decorator.Gof.Visual.Problem;

namespace dev.kaldiroglu.Decorator.Demo;

/// <summary>GoF's two examples, each by subclassing and then by decorating.</summary>
public static class GofExamples
{
    private const string Text = "A decorator conforms to the interface of the component it decorates.";

    // The "aä" is deliberate. Folding ä to a creates a run of two that compression can exploit
    // only if compression runs after folding, so the two orders give different bytes.
    private const string Payload = "Rrrésumé of a naïve café —— aä ee ccc";

    public static void VisualProblem()
    {
        Console.WriteLine("--- problem: one subclass per combination -------------------------------");
        Show("new BorderedTextView(...)",
            VisualProblems.TextView.Render(new VisualProblems.BorderedTextView(30, 4, Text)));
        Show("new ScrolledTextView(...)",
            VisualProblems.TextView.Render(new VisualProblems.ScrolledTextView(30, 4, Text)));
        Show("new BorderedScrolledTextView(...)",
            VisualProblems.TextView.Render(new VisualProblems.BorderedScrolledTextView(30, 4, Text)));
        Show("new ScrolledBorderedTextView(...)",
            VisualProblems.TextView.Render(new VisualProblems.ScrolledBorderedTextView(30, 4, Text)));
        Console.WriteLine(
            """
            Four subclasses for two embellishments, and the scrollbar is written out
            three times. A third embellishment would need eleven more classes.
            """);
    }

    public static void VisualSolution()
    {
        Console.WriteLine("--- solution: two decorators, composed ----------------------------------");
        IVisualComponent text = new TextView(30, 4, Text);
        Show("new BorderDecorator(text)",
            IVisualComponent.Render(new BorderDecorator(text)));
        Show("new ScrollDecorator(text)",
            IVisualComponent.Render(new ScrollDecorator(text)));
        Show("new BorderDecorator(new ScrollDecorator(text))",
            IVisualComponent.Render(new BorderDecorator(new ScrollDecorator(text))));
        Show("new ScrollDecorator(new BorderDecorator(text))",
            IVisualComponent.Render(new ScrollDecorator(new BorderDecorator(text))));
        Show("new BorderDecorator(new BorderDecorator(text))  — GoF: a property, twice",
            IVisualComponent.Render(new BorderDecorator(new BorderDecorator(text))));
        Console.WriteLine(
            """
            One border class, one scrollbar class, and every combination — including
            combinations nobody anticipated — is available at run time.
            """);
    }

    public static void StreamProblem()
    {
        Console.WriteLine("--- problem: one subclass per (destination x transformation) ------------");
        var file = new StreamProblems.CompressingASCII7FileStream();
        file.PutString(Payload);
        file.Close();
        Console.WriteLine($"CompressingASCII7FileStream -> {file.Contents()}");

        var socket = new StreamProblems.CompressingSocketStream();
        socket.PutString(Payload);
        socket.Close();
        Console.WriteLine($"CompressingSocketStream     -> {socket.Contents()}");
        Console.WriteLine(
            """
            Six classes exist to cover two destinations and two transformations, and
            the two bodies above are identical apart from their base class. The pair
            in the other order would be two classes more.
            """);
    }

    public static void StreamSolution()
    {
        Console.WriteLine("--- solution: two decorators over any destination -----------------------");
        var fileSink = new StreamSolutions.FileStream();
        Stream chain = new StreamSolutions.CompressingStream(new StreamSolutions.ASCII7Stream(fileSink));
        chain.PutString(Payload);
        chain.Close();
        Console.WriteLine($"new CompressingStream(new ASCII7Stream(new FileStream()))  -> {fileSink.Contents()}");

        var socketSink = new StreamSolutions.SocketStream();
        Stream socketChain = new StreamSolutions.CompressingStream(socketSink);
        socketChain.PutString(Payload);
        socketChain.Close();
        Console.WriteLine($"new CompressingStream(new SocketStream())                  -> {socketSink.Contents()}");

        var reversedSink = new StreamSolutions.FileStream();
        Stream reversed = new StreamSolutions.ASCII7Stream(new StreamSolutions.CompressingStream(reversedSink));
        reversed.PutString(Payload);
        reversed.Close();
        Console.WriteLine($"new ASCII7Stream(new CompressingStream(new FileStream()))  -> {reversedSink.Contents()}");
        Console.WriteLine(
            """
            Four classes cover every destination, every transformation and every
            order — including the order in the last line, which nobody had to
            anticipate when the decorators were written.

            Compare the last two file results: folding "aä" to "aa" creates a run
            that compression can only exploit if it runs after the folding, so the
            same two decorators in the other order produce different bytes. With
            subclasses, that second order is another class; here it is another
            pair of parentheses.
            """);
    }

    private static void Show(string caption, string picture)
    {
        Console.WriteLine("\n" + caption);
        Console.WriteLine(picture);
    }
}
