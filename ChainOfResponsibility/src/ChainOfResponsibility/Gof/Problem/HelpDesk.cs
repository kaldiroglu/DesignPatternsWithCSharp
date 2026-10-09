namespace dev.kaldiroglu.ChainOfResponsibility.Gof.Problem;

/// <summary>
/// GoF's motivation, before the pattern: one object knows the help for every widget.
/// <para>
/// The help desk must know every widget by name, and which dialog each one sits in, so it
/// can fall back to the dialog's help and then to the application's. A new widget, or a
/// button moved to another dialog, is an edit to this class.
/// </para>
/// </summary>
public sealed class HelpDesk
{
    public string HelpFor(string widget) => widget switch
    {
        "print button" => "Help: print the document.",
        "ok button" or "printer list" => "Help: the print dialog lets you choose a printer.",
        "print dialog" => "Help: the print dialog lets you choose a printer.",
        _ => "Help: this is the editor. Press F1 on any control."
    };
}
