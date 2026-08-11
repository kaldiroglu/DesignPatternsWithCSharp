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
}
