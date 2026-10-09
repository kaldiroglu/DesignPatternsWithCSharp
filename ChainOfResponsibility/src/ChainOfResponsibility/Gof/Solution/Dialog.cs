namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Solution;

/// <summary>
/// A <b>ConcreteHandler</b>: a dialog. A dialog is not inside another widget, so GoF give it
/// the application as its successor.
/// </summary>
public sealed class Dialog : Widget
{
    public Dialog(Application application, string? topic) : base(application, topic)
    {
    }
}
