namespace dev.kaldiroglu.Decorator.Hw.InvoicePipeline;

using System.Security.Cryptography;

/// <summary>
/// Appends a SHA-256 of everything beneath it, and refuses to return altered bytes.
/// <para>
/// A checksum is worthless unless it covers the bytes that actually travel, so this stage
/// belongs <b>outermost</b>. That is a domain rule, not a preference — and nothing in the
/// type system says so, which is the point of the exercise.
/// </para>
/// </summary>
public sealed class Checksummed : PipelineStage
{
    private const int DigestLength = 32;

    public Checksummed(IPipeline inner) : base(inner)
    {
    }

    protected override byte[] Forward(byte[] bytes)
    {
        var digest = Sha256(bytes);
        var outp = new byte[bytes.Length + DigestLength];
        bytes.CopyTo(outp, 0);
        digest.CopyTo(outp, bytes.Length);
        return outp;
    }

    protected override byte[] Backward(byte[] bytes)
    {
        if (bytes.Length < DigestLength)
        {
            throw new ArgumentException("too short to carry a checksum");
        }

        var split = bytes.Length - DigestLength;
        var payload = bytes[..split];
        var found = bytes[split..];

        if (!Sha256(payload).AsSpan().SequenceEqual(found))
        {
            throw new InvalidOperationException("checksum does not match: the file was altered");
        }

        return payload;
    }

    public static byte[] Sha256(byte[] bytes) => SHA256.HashData(bytes);
}
