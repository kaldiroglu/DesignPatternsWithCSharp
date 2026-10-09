namespace dev.kaldiroglu.ChainOfResponsibility.Tests.Gof;

using dev.kaldiroglu.ChainOfResponsibility.Gof;
using dev.kaldiroglu.ChainOfResponsibility.Gof.Problem;
using dev.kaldiroglu.ChainOfResponsibility.Gof.Solution;
using Xunit;
using static dev.kaldiroglu.ChainOfResponsibility.Tests.Printed;

/// <summary>GoF's context-sensitive help, before and after the pattern.</summary>
public class HelpTest
{
    private const string Editor = "this is the editor. Press F1 on any control.";
    private const string PrintDialog = "the print dialog lets you choose a printer.";

    /// <summary>The print button answers itself.</summary>
    [Fact]
    public void AButtonWithHelp()
    {
        Application editor = new Application(Editor);
        Button print = new Button(new Dialog(editor, PrintDialog), "print the document.");
        Assert.Equal("Help: print the document.", print.HandleHelp());
    }

    /// <summary>The OK button has no help, so its dialog answers and the application is never asked.</summary>
    [Fact]
    public void TheOkButtonGetsTheDialogsHelp()
    {
        Application editor = new Application(Editor);
        Button ok = new Button(new Dialog(editor, PrintDialog));
        Assert.False(ok.HasHelp());
        Assert.Equal("Help: " + PrintDialog, ok.HandleHelp());
    }

    /// <summary>A button on a dialog with no help gets the application's help.</summary>
    [Fact]
    public void TheApplicationAnswersLast()
    {
        Application editor = new Application(Editor);
        Button font = new Button(new Dialog(editor, null));
        Assert.Equal("Help: " + Editor, font.HandleHelp());
    }

    /// <summary>A chain where nobody has help still gives an answer.</summary>
    [Fact]
    public void NobodyHasHelp()
    {
        // The Java passes null to Application. The C# constructor takes a non-nullable
        // string, so null! says the same thing.
        Button lonely = new Button(new Dialog(new Application(null!), null));
        Assert.Equal("No help is available.", lonely.HandleHelp());
    }

    /// <summary>Before the pattern, one help desk knows every control by name.</summary>
    [Fact]
    public void TheHelpDeskKnowsEveryControl()
    {
        HelpDesk desk = new HelpDesk();
        Assert.Equal("Help: print the document.", desk.HelpFor("print button"));
        Assert.Equal("Help: " + PrintDialog, desk.HelpFor("ok button"));
        Assert.Equal("Help: " + Editor, desk.HelpFor("font button"));
        // print button, ok button, printer list and print dialog. The Java counts
        // `case "` and `, "` in a switch statement; the C# switch expression writes
        // each name before `" =>`, and joins two names with `" or "`.
        string code = CodeOf("Gof/Problem/HelpDesk.cs");
        Assert.Equal(4, CountOf(code, "\" =>") + CountOf(code, "\" or \""));
    }

    /// <summary>Main asks three controls in both designs and prints the same lines.</summary>
    [Fact]
    public void MainOutput()
    {
        // Gof.Problem and Gof.Solution have a Main of their own, so the name is written in full.
        IReadOnlyList<string> lines = By(global::dev.kaldiroglu.ChainOfResponsibility.Gof.Main.Run);
        Assert.Equal(new[]
        {
            "Before the pattern",
            "  print button: Help: print the document.",
            "  ok button:    Help: " + PrintDialog,
            "  font button:  Help: " + Editor,
            "With the chain",
            "  print button: Help: print the document.",
            "  ok button:    Help: " + PrintDialog,
            "  font button:  Help: " + Editor
        }, lines);
        Assert.Equal(lines.Skip(1).Take(3), lines.Skip(5).Take(3));
    }
}
