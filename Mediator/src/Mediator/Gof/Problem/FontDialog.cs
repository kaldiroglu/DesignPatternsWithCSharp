namespace dev.kaldiroglu.Mediator.Gof.Problem;

/// <summary>
/// GoF's motivation, before the pattern: a font dialog whose widgets call each other.
/// <para>
/// The list box knows the entry field, the entry field knows the OK button, the OK button
/// knows the dialog. Each widget class is written for this dialog and cannot be used in
/// another one, and the dialog's behavior is spread over all of them.
/// </para>
/// </summary>
public sealed class FontDialog
{
    private readonly List<string> log = [];

    public FontDialog()
    {
        Ok = new Button(this);
        FontName = new EntryField(Ok);
        FontList = new ListBox(FontName);
    }

    /// <summary>What the dialog did, in order. The Java exposes the list itself as a public field.</summary>
    public IList<string> Log => log;

    public ListBox FontList { get; }

    public EntryField FontName { get; }

    public Button Ok { get; }

    internal void Apply(string font)
    {
        log.Add("font set to " + font);
    }

    public sealed class ListBox
    {
        private readonly EntryField fontName;

        internal ListBox(EntryField fontName)
        {
            this.fontName = fontName;
        }

        public void Select(string font)
        {
            fontName.SetText(font);          // the list box updates the entry field itself
        }
    }

    public sealed class EntryField
    {
        private readonly Button ok;

        internal EntryField(Button ok)
        {
            this.ok = ok;
        }

        public void SetText(string text)
        {
            Text = text;
            ok.Enabled = text.Length > 0;    // the entry field enables the button itself
        }

        public string Text { get; private set; } = "";
    }

    public sealed class Button
    {
        private readonly FontDialog dialog;

        internal Button(FontDialog dialog)
        {
            this.dialog = dialog;
        }

        /// <summary>Anyone may read it; only the entry field in this dialog sets it.</summary>
        public bool Enabled { get; internal set; }

        public void Click(string font)
        {
            if (Enabled)
            {
                dialog.Apply(font);
            }
        }
    }
}
