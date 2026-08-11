namespace dev.kaldiroglu.Decorator.Hw.InvoicePipeline;

/// <summary>
/// A keystream XOR — deliberately toy cryptography, so the exercise is about position in
/// the chain rather than about ciphers. Exclusive-or is its own inverse, which is why
/// Forward and Backward are the same operation.
/// </summary>
public sealed class Encrypted : PipelineStage
{
    private readonly int _key;

    public Encrypted(IPipeline inner, int key) : base(inner) => _key = key;

    protected override byte[] Forward(byte[] bytes) => Xor(bytes);

    protected override byte[] Backward(byte[] bytes) => Xor(bytes);

    private byte[] Xor(byte[] bytes)
    {
        var keystream = new Random(_key);
        var outp = new byte[bytes.Length];
        for (var i = 0; i < bytes.Length; i++)
        {
            outp[i] = (byte)(bytes[i] ^ (byte)keystream.Next(256));
        }

        return outp;
    }
}
