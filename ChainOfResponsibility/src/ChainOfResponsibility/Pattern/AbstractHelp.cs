namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>A help text and a link to other help.</summary>
public abstract class AbstractHelp : IHelp
{
    protected string description = "";
    protected IHelp? otherHelp;

    public string Description => description;

    public IHelp? OtherHelp => otherHelp;

    public void AddHelp(IHelp help)
    {
        if (otherHelp == null)
        {
            otherHelp = help;
        }
        else
        {
            otherHelp.AddHelp(help);
        }
    }

    public void Show()
    {
        Console.WriteLine(description);
        otherHelp?.Show();
    }
}
