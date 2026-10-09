namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>The answer: a help text that can hold other help.</summary>
public interface IHelp
{
    void Show();

    /// <summary>Adds help at the end of this help's list, so no help already there is replaced.</summary>
    void AddHelp(IHelp help);

    IHelp? OtherHelp { get; }
}
