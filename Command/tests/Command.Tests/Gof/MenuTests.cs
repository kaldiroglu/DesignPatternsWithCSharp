namespace dev.kaldiroglu.Command.Tests.Gof;

using System.Reflection;
using System.Text.RegularExpressions;
using dev.kaldiroglu.Command.Gof;
using dev.kaldiroglu.Command.Gof.Solution;
using Xunit;
using ProblemMenuItem = dev.kaldiroglu.Command.Gof.Problem.MenuItem;

/// <summary>
/// GoF's menu example, before and after the pattern. Ported from the Java
/// <c>gof.MenuTest</c>.
/// </summary>
public class MenuTests
{
    private const string Source = "Gof/";

    private const BindingFlags Declared =
        BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public
        | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    /// <summary>The application's classes: the receivers.</summary>
    private static readonly Type[] Receivers = [typeof(Application), typeof(Document), typeof(Clipboard)];

    /// <summary>
    /// Whether the source of a file, comments removed, names one of these types as a whole
    /// word. Java checks the <c>import</c> lines. C# needs no <c>using</c> line here: the
    /// toolkit classes sit in a namespace inside <c>Gof</c>, so they see Application and
    /// Document without one. The closest check is the code itself.
    /// </summary>
    private static bool CodeNames(string relativePath, string typeName)
    {
        var code = SourceText.StripComments(SourceText.Read(Source + relativePath));
        return Regex.IsMatch(code, @"\b" + typeName + @"\b");
    }

    /// <summary>Every type that a class's fields, constructors, methods and properties mention.</summary>
    private static IEnumerable<Type> TypesInSignatures(Type type) =>
        type.GetFields(Declared).Select(f => f.FieldType)
            .Concat(type.GetProperties(Declared).Select(p => p.PropertyType))
            .Concat(type.GetConstructors(Declared).SelectMany(c => c.GetParameters()).Select(p => p.ParameterType))
            .Concat(type.GetMethods(Declared).SelectMany(m => m.GetParameters()).Select(p => p.ParameterType))
            .Concat(type.GetMethods(Declared).Select(m => m.ReturnType));

    /// <summary>
    /// A command made from a lambda. Java's <c>Command</c> is a functional interface, so its
    /// tests write commands as lambdas; a C# lambda cannot implement <c>ICommand</c>.
    /// </summary>
    private sealed class Run(Action action) : ICommand
    {
        public void Execute() => action();
    }

    // ------------------------------------------------------------ without the pattern

    [Fact(DisplayName = "before the pattern: the toolkit's menu item imports the application's classes")]
    public void TheProblemMenuItemImportsTheApplication()
    {
        Assert.True(CodeNames("Problem/MenuItem.cs", "Application"));
        Assert.True(CodeNames("Problem/MenuItem.cs", "Document"));
        Assert.Contains(typeof(Application), TypesInSignatures(typeof(ProblemMenuItem)));
    }

    [Fact(DisplayName = "before the pattern: the menu item branches on its label for Open, Copy and Paste")]
    public void TheProblemMenuItemWorksForItsThreeLabels()
    {
        var application = new Application();
        new ProblemMenuItem("Open", application, () => "letter").Clicked();
        var letter = application.Current();
        letter.Type("Dear Deniz");

        new ProblemMenuItem("Copy", application, () => "").Clicked();
        new ProblemMenuItem("Paste", application, () => "").Clicked();

        Assert.Equal("letter", letter.Name);
        Assert.True(letter.IsOpen);
        Assert.Equal("Dear DenizDear Deniz", letter.Text);
    }

    [Fact(DisplayName = "before the pattern: a menu entry spelled paste compiles and fails on the first click")]
    public void AMisspelledLabelFailsOnTheFirstClick()
    {
        var application = new Application();
        application.Add(new Document("letter", application.Clipboard));
        var paste = new ProblemMenuItem("paste", application, () => "");

        Assert.Throws<InvalidOperationException>(paste.Clicked);
    }

    // ---------------------------------------------------------------- with the pattern

    [Fact(DisplayName = "with the pattern: the menu item imports nothing from the application")]
    public void TheSolutionMenuItemImportsNothingFromTheApplication()
    {
        foreach (var (file, type) in new[]
                 {
                     ("Solution/MenuItem.cs", typeof(MenuItem)),
                     ("Solution/Menu.cs", typeof(Menu)),
                     ("Solution/ICommand.cs", typeof(ICommand))
                 })
        {
            foreach (var receiver in Receivers)
            {
                Assert.False(CodeNames(file, receiver.Name), file + " must not name " + receiver.Name);
                Assert.DoesNotContain(receiver, TypesInSignatures(type));
            }
        }
    }

    [Fact(DisplayName = "with the pattern: the menu item has no switch on its label")]
    public void TheSolutionMenuItemHasNoSwitch()
    {
        var code = SourceText.StripComments(SourceText.Read(Source + "Solution/MenuItem.cs"));

        Assert.DoesNotContain("switch", code);
        Assert.DoesNotContain("\"Paste\"", code);
    }

    [Fact(DisplayName = "the command interface has one method, execute, with no arguments")]
    public void CommandHasOneMethodWithNoArguments()
    {
        var methods = typeof(ICommand).GetMethods();

        Assert.Single(methods);
        Assert.Equal("Execute", methods[0].Name);
        Assert.Empty(methods[0].GetParameters());
    }

    [Fact(DisplayName = "a paste command forwards to the document it was given")]
    public void PasteForwardsToItsDocument()
    {
        var application = new Application();
        var letter = new Document("letter", application.Clipboard);
        letter.Type("Hello");
        letter.Copy();
        var paste = new MenuItem("Paste", new PasteCommand(letter));

        paste.Clicked();

        Assert.Equal("HelloHello", letter.Text);
    }

    [Fact(DisplayName = "an open command asks for a name, creates a document, adds it and opens it")]
    public void OpenCreatesAddsAndOpens()
    {
        var application = new Application();
        var open = new MenuItem("Open", new OpenCommand(application, () => "report"));

        open.Clicked();

        Assert.Single(application.Documents());
        var report = application.Current();
        Assert.Equal("report", report.Name);
        Assert.True(report.IsOpen);
    }

    [Fact(DisplayName = "an open command does nothing when the user cancels the dialog")]
    public void OpenDoesNothingWhenCancelled()
    {
        var application = new Application();

        new OpenCommand(application, () => null).Execute();
        new OpenCommand(application, () => "  ").Execute();

        Assert.Empty(application.Documents());
    }

    [Fact(DisplayName = "a macro command runs its commands in order, and a menu item cannot tell it apart")]
    public void AMacroRunsItsCommandsInOrder()
    {
        var ran = new List<string>();
        var macro = new MacroCommand()
            .Add(new Run(() => ran.Add("first")))
            .Add(new Run(() => ran.Add("second")))
            .Add(new Run(() => ran.Add("third")));
        var item = new MenuItem("Do all", macro);

        item.Clicked();

        Assert.Equal(["first", "second", "third"], ran);
        Assert.Equal(3, macro.Size);
    }

    [Fact(DisplayName = "a removed command is no longer run by the macro")]
    public void ARemovedCommandIsNotRun()
    {
        var ran = new List<string>();
        ICommand second = new Run(() => ran.Add("second"));
        var macro = new MacroCommand().Add(new Run(() => ran.Add("first"))).Add(second);

        macro.Remove(second);
        macro.Execute();

        Assert.Equal(["first"], ran);
    }

    [Fact(DisplayName = "SimpleCommand with Document::paste does what PasteCommand does")]
    public void SimpleCommandIsPasteWithoutAClass()
    {
        var application = new Application();
        var byClass = new Document("a", application.Clipboard);
        var byReference = new Document("b", application.Clipboard);
        application.Clipboard.Put("text");

        new PasteCommand(byClass).Execute();
        new SimpleCommand<Document>(byReference, d => d.Paste()).Execute();

        Assert.Equal("text", byClass.Text);
        Assert.Equal(byClass.Text, byReference.Text);
    }

    [Fact(DisplayName = "the same menu item can be given a different job while the program runs")]
    public void SetCommandChangesTheJob()
    {
        var ran = new List<string>();
        var item = new MenuItem("Action", new Run(() => ran.Add("old job")));

        item.Clicked();
        item.SetCommand(new Run(() => ran.Add("new job")));
        item.Clicked();

        Assert.Equal(["old job", "new job"], ran);
        Assert.Throws<ArgumentNullException>(() => item.SetCommand(null!));
    }

    [Fact(DisplayName = "a menu clicks the item with that label, and refuses a label it does not have")]
    public void AMenuFindsItsItemByLabel()
    {
        var ran = new List<string>();
        var menu = new Menu()
            .Add(new MenuItem("Open", new Run(() => ran.Add("open"))))
            .Add(new MenuItem("Paste", new Run(() => ran.Add("paste"))));

        menu.Click("Paste");

        Assert.Equal(["paste"], ran);
        Assert.Equal(["Open", "Paste"], menu.Labels());
        Assert.Throws<ArgumentException>(() => menu.Click("paste"));
    }
}
