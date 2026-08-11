namespace dev.kaldiroglu.Decorator.Hw.InvoicePipeline;

using System.Text;

/// <summary>Prepends a header, and refuses to read a file that does not carry it.</summary>
public sealed class WithHeader : PipelineStage
{
    private readonly byte[] _header;

    public WithHeader(IPipeline inner, string header) : base(inner) =>
        _header = Encoding.UTF8.GetBytes(header + "\n");

    protected override byte[] Forward(byte[] bytes)
    {
        var outp = new byte[_header.Length + bytes.Length];
        _header.CopyTo(outp, 0);
        bytes.CopyTo(outp, _header.Length);
        return outp;
    }

    protected override byte[] Backward(byte[] bytes)
    {
        if (bytes.Length < _header.Length || !bytes.AsSpan(0, _header.Length).SequenceEqual(_header))
        {
            throw new ArgumentException("header missing: this chain is not the mirror "
                                        + "of the one that wrote the file");
        }

        return bytes[_header.Length..];
    }
}
