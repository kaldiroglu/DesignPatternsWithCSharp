namespace dev.kaldiroglu.Decorator.Hw.InvoicePipeline;

/// <summary>The <b>ConcreteComponent</b>: the invoice, untouched.</summary>
public sealed class PlainInvoice : IPipeline
{
    public byte[] Process(byte[] input) => (byte[])input.Clone();

    public byte[] Undo(byte[] input) => (byte[])input.Clone();
}
