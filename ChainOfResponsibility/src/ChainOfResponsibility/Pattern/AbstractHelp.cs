namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>A help text and a link to other help.</summary>
public abstract class AbstractHelp : IHelp
{
    protected string description = "";
    protected IHelp? otherHelp;

    public string Description => description;

    public IHelp? OtherHelp
    {
        get => otherHelp;
        set => otherHelp = value;
    }

    public void Show()
    {
        Console.WriteLine(description);
        // NOTE: this follows at most two links: the other help, and the other help's other
        // help. A longer list of help is cut off. The Java has the same behavior.
        if (otherHelp != null)
        {
            otherHelp.Show();
            IHelp? otherOtherHelp = otherHelp.OtherHelp;
            if (otherOtherHelp != null)
            {
                otherOtherHelp.Show();
            }
        }
    }
}
