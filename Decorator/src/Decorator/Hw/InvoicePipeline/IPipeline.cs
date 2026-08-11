namespace dev.kaldiroglu.Decorator.Hw.InvoicePipeline;

/// <summary>
/// Homework 1, the invoice pipeline. The <b>Component</b>: bytes in, bytes out — and the
/// mirror image that reads them back.
/// </summary>
public interface IPipeline
{
    byte[] Process(byte[] input);

    byte[] Undo(byte[] input);
}
