namespace dev.kaldiroglu.State.Pattern;

/// <summary>The <b>Context</b> of the outline. It holds a state, and its methods are still empty.</summary>
public class ConcreteTCPConnection : ITCPConnection
{
    // The outline never uses this field, as in the Java. CS0169 is "the field is never used".
#pragma warning disable CS0169
    private ITCPState? state;
#pragma warning restore CS0169

    public void Open()
    {
    }

    public void Close()
    {
    }

    public void Acknowledge()
    {
    }
}
