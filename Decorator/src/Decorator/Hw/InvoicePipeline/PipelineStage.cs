namespace dev.kaldiroglu.Decorator.Hw.InvoicePipeline;

/// <summary>
/// The <b>Decorator</b>. Note <see cref="Undo"/>: it unwraps in the opposite order to
/// <see cref="Process"/>, which is the obligation a chain places on whoever reads the file
/// back. Every layer added on the way out needs its counterpart on the way in.
/// </summary>
public abstract class PipelineStage : IPipeline
{
    protected readonly IPipeline Inner;

    protected PipelineStage(IPipeline inner) =>
        Inner = inner ?? throw new ArgumentNullException(nameof(inner), "a stage must wrap something");

    protected abstract byte[] Forward(byte[] bytes);

    protected abstract byte[] Backward(byte[] bytes);

    public byte[] Process(byte[] input) => Forward(Inner.Process(input));

    public byte[] Undo(byte[] input) => Inner.Undo(Backward(input));
}
