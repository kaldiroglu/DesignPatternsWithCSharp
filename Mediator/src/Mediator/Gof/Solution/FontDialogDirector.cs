namespace dev.kaldiroglu.Mediator.Gof.Solution;

/// <summary>
/// The <b>ConcreteMediator</b>: GoF's <c>FontDialogDirector</c>. It creates the widgets and
/// holds the whole behavior of the dialog in one method.
/// <para>
/// The widgets are general: the same <see cref="ListBox"/>, <see cref="EntryField"/> and
/// <see cref="Button"/> could be used in any other dialog with a different director.
/// </para>
/// </summary>
public sealed class FontDialogDirector : DialogDirector
{
    private readonly List<string> log = [];

    public FontDialogDirector()
    {
        FontList = new ListBox(this);
        FontName = new EntryField(this);
        Ok = new Button(this);
        Cancel = new Button(this);
        Cancel.Enabled = true;
    }

    /// <summary>What the dialog did, in order. The Java exposes the list itself as a public field.</summary>
    public IList<string> Log => log;

    public ListBox FontList { get; }

    public EntryField FontName { get; }

    public Button Ok { get; }

    public Button Cancel { get; }

    public override void WidgetChanged(Widget widget)
    {
        if (widget == FontList)
        {
            FontName.SetText(FontList.Selection);
            Ok.Enabled = true;
        }
        else if (widget == FontName)
        {
            Ok.Enabled = FontName.Text.Length > 0;
        }
        else if (widget == Ok)
        {
            log.Add("font set to " + FontName.Text);
        }
        else if (widget == Cancel)
        {
            log.Add("dialog closed");
        }
    }
}
