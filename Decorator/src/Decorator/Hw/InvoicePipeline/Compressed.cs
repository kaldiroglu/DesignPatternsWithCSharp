namespace dev.kaldiroglu.Decorator.Hw.InvoicePipeline;

using System.IO.Compression;

/// <summary>
/// Gzip. Its position matters twice over: encrypted bytes do not compress, so this must be
/// inside the encryption, and a checksum must cover the bytes that actually travel.
/// </summary>
public sealed class Compressed : PipelineStage
{
    public Compressed(IPipeline inner) : base(inner)
    {
    }

    protected override byte[] Forward(byte[] bytes)
    {
        using var outp = new MemoryStream();
        using (var gzip = new GZipStream(outp, CompressionMode.Compress, leaveOpen: true))
        {
            gzip.Write(bytes, 0, bytes.Length);
        }

        return outp.ToArray();
    }

    protected override byte[] Backward(byte[] bytes)
    {
        using var input = new MemoryStream(bytes);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var outp = new MemoryStream();
        gzip.CopyTo(outp);
        return outp.ToArray();
    }
}
