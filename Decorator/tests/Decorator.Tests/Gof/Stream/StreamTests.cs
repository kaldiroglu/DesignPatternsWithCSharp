using dev.kaldiroglu.Decorator.Gof.Stream;
using dev.kaldiroglu.Decorator.Gof.Stream.Problem;
using Xunit;
using SolutionAscii = dev.kaldiroglu.Decorator.Gof.Stream.Solution.ASCII7Stream;
using SolutionCompressing = dev.kaldiroglu.Decorator.Gof.Stream.Solution.CompressingStream;
using SolutionFile = dev.kaldiroglu.Decorator.Gof.Stream.Solution.FileStream;
using SolutionSocket = dev.kaldiroglu.Decorator.Gof.Stream.Solution.SocketStream;

namespace dev.kaldiroglu.Decorator.Tests.Gof.Stream;

/// <summary>
/// GoF's second Sample Code example. The same pattern with no graphics in it at all, which
/// is why the book gives two.
/// </summary>
public class StreamTests
{
    [Fact(DisplayName = "the codecs do what the streams claim")]
    public void Codecs()
    {
        Assert.Equal("3a", global::dev.kaldiroglu.Decorator.Gof.Stream.Codecs.Compress("aaa"));
        Assert.Equal("cafe", global::dev.kaldiroglu.Decorator.Gof.Stream.Codecs.ToAscii7("café"));
    }

    [Fact(DisplayName = "subclassing and decorating write the same bytes")]
    public void SameOutput()
    {
        var bySubclassing = new CompressingASCII7FileStream(8);
        bySubclassing.PutInt(12);
        bySubclassing.PutString(" aaa café");
        bySubclassing.Close();

        var file = new SolutionFile(8);
        var byDecorating = new SolutionAscii(new SolutionCompressing(file, 8), 8);
        byDecorating.PutInt(12);
        byDecorating.PutString(" aaa café");
        byDecorating.Close();

        Assert.Equal(bySubclassing.Contents(), file.Contents());
    }

    [Fact(DisplayName = "the same decorator over a different destination, and no new class")]
    public void TheSameDecoratorElsewhere()
    {
        var socket = new SolutionSocket(8);
        var toSocket = new SolutionCompressing(socket, 8);
        toSocket.PutString("aaabbb");
        toSocket.Close();

        var file = new SolutionFile(8);
        var toFile = new SolutionCompressing(file, 8);
        toFile.PutString("aaabbb");
        toFile.Close();

        Assert.Equal("3a3b", socket.Contents());
        Assert.Equal(socket.Contents(), file.Contents());
    }

    [Fact(DisplayName = "closing the outermost stream closes every stream beneath it")]
    public void CloseIsForwarded()
    {
        var file = new SolutionFile(1024);
        var chain = new SolutionAscii(new SolutionCompressing(file, 1024), 1024);
        chain.PutString("café");
        chain.Close(); // nobody holds a reference to `file` except the chain

        Assert.Equal("cafe", file.Contents());
    }

    [Fact(DisplayName = "two axes: transformations x destinations, added instead of multiplied")]
    public void TheArithmetic()
    {
        // Problem: FileStream, SocketStream, CompressingFileStream, ASCII7FileStream,
        //          CompressingASCII7FileStream, CompressingSocketStream = 6 for 2 x 2.
        // Solution: 2 destinations + 2 transformations + 1 base = 5, and every pairing
        //          and order of them is free.
        Assert.Equal(6, 2 * 2 + 2);   // m x n, plus the two bare destinations
        Assert.Equal(5, 2 + 2 + 1);   // m + n
    }
}
