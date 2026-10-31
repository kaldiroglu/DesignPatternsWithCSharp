using System.IO.Compression;
using System.Text;
using dev.kaldiroglu.Decorator.Io;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Io;

/// <summary>
/// The pattern in the standard library, measured: the same invoice written through two
/// chains that differ by one decorator.
/// </summary>
public class InvoiceStreamTests
{
    [Fact(DisplayName = "one more decorator, a smaller file, and the same total")]
    public void CompressionIsOneMoreLayer()
    {
        var (plainBytes, gzippedBytes, total) = InvoiceStreamDemo.Run(print: false);

        Assert.True(gzippedBytes < plainBytes,
            $"gzip should be smaller: {gzippedBytes} vs {plainBytes}");
        Assert.Equal(InvoiceStreamDemo.ExpectedTotal, total, 2);
    }

    [Fact(DisplayName = "the plain file is 274 bytes, and the gzipped file depends on the library")]
    public void TheFileSizes()
    {
        var (plainBytes, gzippedBytes, _) = InvoiceStreamDemo.Run(print: false);

        Assert.Equal(274, plainBytes);
        Assert.Equal(194, gzippedBytes);
    }

    [Fact(DisplayName = "the same bytes in one write at the smallest size take 186 bytes")]
    public void WithoutTheFlushAndAtTheSmallestSizeTheFileIs186Bytes()
    {
        var invoice = new MemoryStream();
        using (var writer = new BinaryWriter(invoice, Encoding.BigEndianUnicode, leaveOpen: true))
        {
            InvoiceStreamDemo.WriteInvoice(writer);
        }

        var compressed = new MemoryStream();
        using (var gzip = new GZipStream(compressed, CompressionLevel.SmallestSize, leaveOpen: true))
        {
            gzip.Write(invoice.ToArray());
        }

        Assert.Equal(186, compressed.Length);
    }
}
