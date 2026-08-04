using dev.kaldiroglu.Composite.Hw.SurveyForm;
using Xunit;

namespace dev.kaldiroglu.Composite.Tests.Hw;

/// <summary>
/// Homework 2: an operation that concatenates rather than sums, and the transparent variant's
/// run-time failure.
/// </summary>
public class SurveyFormTests
{
    private readonly IFormElement _form = new Section("Course feedback").With(
        new Section("About you").With(
            new Question("Name", false).Answer("Bora"),
            new Question("Role", true).Answer("engineer"),
            new Question("Years of experience", true)),
        new Section("The session").With(
            new Question("Which pattern was clearest?", true).Answer("Composite"),
            new Question("What should we cut?", false)));

    [Fact(DisplayName = "Counts roll up through nested sections")]
    public void CountsRollUp()
    {
        Assert.Equal(5, _form.QuestionCount());
        Assert.Equal(3, _form.AnsweredCount());
    }

    [Fact(DisplayName = "Validation concatenates the children's problems")]
    public void ValidationConcatenates()
    {
        var problems = _form.Validate();

        Assert.Single(problems);
        Assert.Contains("Years of experience", problems[0]);
    }

    [Fact(DisplayName = "A leaf validates itself")]
    public void ALeafValidatesItself()
    {
        Assert.Single(new Question("Role", true).Validate());
        Assert.Empty(new Question("Role", true).Answer("engineer").Validate());
        Assert.Empty(new Question("Optional", false).Validate());
    }

    [Fact(DisplayName = "The transparent variant: Add on a Question compiles, then throws")]
    public void AddOnALeafThrowsAtRunTime()
    {
        IFormElement question = new Question("Name", false);

        var failure = Assert.Throws<NotSupportedException>(
            () => question.Add(new Question("Nested", false)));
        Assert.Contains("no children", failure.Message);
    }

    [Fact(DisplayName = "A section cannot contain itself")]
    public void SelfContainmentIsRejected()
    {
        var section = new Section("About you");
        Assert.Throws<ArgumentException>(() => section.Add(section));
    }
}
