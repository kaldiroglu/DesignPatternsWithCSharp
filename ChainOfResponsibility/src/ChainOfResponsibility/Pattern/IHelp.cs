namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>The answer: a help text that can hold other help.</summary>
public interface IHelp
{
    void Show();

    IHelp? OtherHelp { get; set; }
}
