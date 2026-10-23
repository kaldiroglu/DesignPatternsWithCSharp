namespace dev.kaldiroglu.Mediator.Tests.Gof;

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

    /// <summary>Both designs give the same field, the same OK button and the same result for the same steps.</summary>
    [Fact]
    public void BothDesignsBehaveTheSame()
    {
        FontDialog before = new FontDialog();
        FontDialogDirector after = new FontDialogDirector();
        before.FontList.Select("Helvetica");
        after.FontList.Select("Helvetica");
        Assert.Equal(before.FontName.Text, after.FontName.Text);
        Assert.Equal(before.Ok.Enabled, after.Ok.Enabled);

        before.FontName.SetText("");
        after.FontName.Type("");
        Assert.Equal(before.Ok.Enabled, after.Ok.Enabled);

        before.FontName.SetText("Times");
        before.Ok.Click(before.FontName.Text);
        after.FontName.Type("Times");
        after.Ok.Click();
        Assert.Equal(before.Log, after.Log);
    }

    /// <summary>The problem's Main prints the three steps without a director.</summary>
    [Fact]
    public void ProblemMainOutput()
    {
        Assert.Equal(new[]
        {
            "After selecting: field 'Helvetica', OK enabled true",
            "After clearing:  OK enabled false",
            "After OK:        [font set to Times]",
            "ListBox holds the EntryField, and EntryField holds the Button."
        }, By(global::dev.kaldiroglu.Mediator.Gof.Problem.Main.Run));
    }

    /// <summary>The solution's Main prints the same three steps with a director, then Cancel.</summary>
    [Fact]
    public void SolutionMainOutput()
    {
        Assert.Equal(new[]
        {
            "After selecting: field 'Helvetica', OK enabled true",
            "After clearing:  OK enabled false",
            "After OK and Cancel: [font set to Times, dialog closed]"
        }, By(global::dev.kaldiroglu.Mediator.Gof.Solution.Main.Run));
    }
}
