using System.Text;
using dev.kaldiroglu.Decorator.Hw.InvoicePipeline;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Hw;

/// <summary>
/// Homework 1. Two of the six orderings are simply wrong, and nothing in the type system
/// says so.
/// </summary>
public class InvoicePipelineTests
{
    private static readonly byte[] Invoice =
        Encoding.UTF8.GetBytes(string.Concat(Enumerable.Repeat("INVOICE LINE 1234567890\n", 80)));

    [Fact(DisplayName = "a chain and its mirror image round-trip the bytes")]
    public void RoundTrip()
    {
        IPipeline chain = new Checksummed(
            new Encrypted(new Compressed(new WithHeader(new PlainInvoice(), "ACME")), 42));

        var written = chain.Process(Invoice);
        Assert.Equal(Invoice, chain.Undo(written));
    }

    [Fact(DisplayName = "encrypted bytes do not compress: order decides the size")]
    public void EncryptedBytesDoNotCompress()
    {
        var compressThenEncrypt = new Encrypted(new Compressed(new PlainInvoice()), 42)
            .Process(Invoice).Length;
        var encryptThenCompress = new Compressed(new Encrypted(new PlainInvoice(), 42))
            .Process(Invoice).Length;

        Assert.True(compressThenEncrypt < encryptThenCompress,
            $"compressing first should win: {compressThenEncrypt} vs {encryptThenCompress}");
        Assert.True(compressThenEncrypt < Invoice.Length); // and it did compress
    }

    [Fact(DisplayName = "a checksum must cover the bytes that travel, so it belongs outermost")]
    public void TheChecksumCoversWhatTravels()
    {
        IPipeline chain = new Checksummed(new Compressed(new PlainInvoice()));
        var written = chain.Process(Invoice);

        written[0] ^= 0xFF; // a single flipped bit anywhere in the payload
        Assert.Throws<InvalidOperationException>(() => chain.Undo(written));
    }

    [Fact(DisplayName = "reading with the wrong chain is refused, not silently wrong")]
    public void TheWrongChainIsRefused()
    {
        var written = new WithHeader(new PlainInvoice(), "ACME").Process(Invoice);

        Assert.Throws<ArgumentException>(() =>
            new WithHeader(new PlainInvoice(), "OTHER").Undo(written));
    }

    [Fact(DisplayName = "a stage must wrap something")]
    public void NullInnerIsRejected() =>
        Assert.Throws<ArgumentNullException>(() => new Compressed(null!));
}
