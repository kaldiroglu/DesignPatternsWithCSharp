namespace dev.kaldiroglu.Mediator.Tests.Gof;

using dev.kaldiroglu.Mediator.Gof;
using dev.kaldiroglu.Mediator.Gof.Problem;
using dev.kaldiroglu.Mediator.Gof.Solution;
using Xunit;
using static dev.kaldiroglu.Mediator.Tests.Fields;
using static dev.kaldiroglu.Mediator.Tests.Printed;

/// <summary>GoF's font dialog, before and after the pattern.</summary>
public class FontDialogTest
{
    /// <summary>With a director: selecting a font fills the field and enables OK.</summary>
    [Fact]
    public void SelectingAFont()
    {
        FontDialogDirector dialog = new FontDialogDirector();
        Assert.False(dialog.Ok.Enabled);
        dialog.FontList.Select("Helvetica");
        Assert.Equal("Helvetica", dialog.FontName.Text);
        Assert.True(dialog.Ok.Enabled);
    }

    /// <summary>With a director: clearing the field disables OK, and a disabled OK does nothing.</summary>
    [Fact]
    public void ClearingTheField()
    {
        FontDialogDirector dialog = new FontDialogDirector();
        dialog.FontList.Select("Helvetica");
        dialog.FontName.Type("");
        dialog.Ok.Click();
        Assert.False(dialog.Ok.Enabled);
        Assert.Empty(dialog.Log);
    }

    /// <summary>With a director: typing Times and clicking OK sets the font to Times; cancel closes the dialog.</summary>
    [Fact]
    public void ClickingOk()
    {
        FontDialogDirector dialog = new FontDialogDirector();
        dialog.FontName.Type("Times");
        dialog.Ok.Click();
        dialog.Cancel.Click();
        Assert.Equal(new[] { "font set to Times", "dialog closed" }, dialog.Log);
    }

    /// <summary>Before the pattern each widget holds the next one; after it each widget holds only the director.</summary>
    [Fact]
    public void WhoKnowsWhom()
    {
        Assert.Contains(typeof(FontDialog.EntryField), HeldBy(typeof(FontDialog.ListBox)));
        Assert.Contains(typeof(FontDialog.Button), HeldBy(typeof(FontDialog.EntryField)));
        Assert.Contains(typeof(FontDialog), HeldBy(typeof(FontDialog.Button)));
        Type[] widgets = [typeof(ListBox), typeof(EntryField), typeof(Button)];
        foreach (Type widget in widgets)
        {
            Assert.False(HoldsAny(widget, widgets), widget.Name);
        }
        Assert.Equal(new[] { typeof(DialogDirector) }, HeldBy(typeof(Widget)));
    }

    /// <summary>Main runs the same steps in both designs and prints the same lines.</summary>
    [Fact]
    public void MainOutput()
    {
        IReadOnlyList<string> lines = By(Main.Run);
        Assert.Equal(new[]
        {
            "Before the pattern",
            "  after selecting: field 'Helvetica', OK enabled true",
            "  after clearing:  OK enabled false",
            "  after OK:        [font set to Times]",
            "With a director",
            "  after selecting: field 'Helvetica', OK enabled true",
            "  after clearing:  OK enabled false",
            "  after OK:        [font set to Times]"
        }, lines);
    }
}
