namespace dev.kaldiroglu.Composite.Hw.SurveyForm;

/// <summary>Homework 2 — the survey form.</summary>
public static class SurveyFormDemo
{
    public static void Run()
    {
        IFormElement form = new Section("Course feedback").With(
            new Section("About you").With(
                new Question("Name", false).Answer("Bora"),
                new Question("Role", true).Answer("engineer"),
                new Question("Years of experience", true)),
            new Section("The session").With(
                new Question("Which pattern was clearest?", true).Answer("Composite"),
                new Question("What should we cut?", false),
                new Section("Exercises").With(
                    new Question("Was the homework the right length?", true),
                    new Question("Anything else?", false))));

        Console.WriteLine(form.Render(""));
        Console.WriteLine();
        Console.WriteLine($"{form.AnsweredCount()} of {form.QuestionCount()} answered");

        Console.WriteLine();
        Console.WriteLine("validation problems, gathered from the whole tree:");
        foreach (var problem in form.Validate())
        {
            Console.WriteLine($"  - {problem}");
        }

        Console.WriteLine();
        Console.WriteLine("Every element above was handled through IFormElement. Nothing asked");
        Console.WriteLine("whether it held a section or a question, at any depth.");
        Console.WriteLine();
        Console.WriteLine("That is what transparency buys. Here is what it costs:");
        try
        {
            new Question("Name", false).Add(new Question("Nested", false));
        }
        catch (NotSupportedException e)
        {
            Console.WriteLine($"  {e.Message}");
            Console.WriteLine("  ...and that line compiled without complaint.");
        }
    }
}
