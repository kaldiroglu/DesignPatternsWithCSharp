namespace dev.kaldiroglu.ChainOfResponsibility.Tests.Gof.Parent;

using System.Reflection;
using dev.kaldiroglu.ChainOfResponsibility.Gof.Parent;
using Xunit;
using static dev.kaldiroglu.ChainOfResponsibility.Tests.Printed;
using Application = dev.kaldiroglu.ChainOfResponsibility.Gof.Solution.Application;
using Button = dev.kaldiroglu.ChainOfResponsibility.Gof.Solution.Button;
using Dialog = dev.kaldiroglu.ChainOfResponsibility.Gof.Solution.Dialog;
using ParentMain = dev.kaldiroglu.ChainOfResponsibility.Gof.Parent.Main;

/// <summary>The help chain made from the parent field the window already has.</summary>
public class ParentLinkTest
{
    private const string Editor = "this is the editor. Press F1 on any control.";
    private const string PrintDialog = "the print dialog lets you choose a printer.";

    [Fact(DisplayName = "the three buttons get the same help as with a successor field")]
    public void SameAnswersAsTheSuccessorField()
    {
        var editor = new Widget(null, "editor", Editor);
        var printDialog = new Widget(editor, "print dialog", PrintDialog);
        string[] parent =
        [
            new Widget(printDialog, "print button", "print the document.").HandleHelp(),
            new Widget(printDialog, "OK button").HandleHelp(),
            new Widget(new Widget(editor, "font dialog"), "font button").HandleHelp(),
        ];

        var app = new Application(Editor);
        var dialog = new Dialog(app, PrintDialog);
        string[] successor =
        [
            new Button(dialog, "print the document.").HandleHelp(),
            new Button(dialog).HandleHelp(),
            new Button(new Dialog(app, null)).HandleHelp(),
        ];

        Assert.Equal(successor, parent);
    }

    [Fact(DisplayName = "the only field that holds another widget is parent")]
    public void OneLinkField()
    {
        var links = typeof(Widget)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(f => f.FieldType == typeof(Widget))
            .Select(f => f.Name)
            .ToList();
        Assert.Equal(["parent"], links);
    }

    [Fact(DisplayName = "Path and HandleHelp both follow parent")]
    public void TwoMethodsUseTheParent()
    {
        string code = CodeOf("Gof/Parent/Widget.cs");
        Assert.Equal(1, CountOf(code, "return parent.Path() + \" > \" + name;"));
        Assert.Equal(1, CountOf(code, "return parent.HandleHelp();"));
    }

    [Fact(DisplayName = "Main prints where each button is, then its help")]
    public void MainOutput()
    {
        Assert.Equal(
        [
            "editor > print dialog > print button: Help: print the document.",
            "editor > print dialog > OK button: Help: " + PrintDialog,
            "editor > font dialog > font button: Help: " + Editor,
        ], By(ParentMain.Run));
    }
}
