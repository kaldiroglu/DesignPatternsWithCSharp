namespace dev.kaldiroglu.Composite.Hw.SurveyForm;

/// <summary>A Leaf: one question, which may or may not have been answered.</summary>
public sealed class Question(string title, bool required) : IFormElement
{
    private string? _answer;

    public string Title { get; } = title ?? throw new ArgumentNullException(nameof(title));

    public Question Answer(string answer)
    {
        _answer = answer;
        return this;
    }

    public IReadOnlyList<string> Validate()
    {
        var missing = required && string.IsNullOrWhiteSpace(_answer);
        return missing ? [$"required and unanswered: {Title}"] : [];
    }

    public int QuestionCount() => 1;

    public int AnsweredCount() => string.IsNullOrWhiteSpace(_answer) ? 0 : 1;

    public string Render(string indent)
    {
        var mark = string.IsNullOrWhiteSpace(_answer) ? (required ? "[ ] *" : "[ ]  ") : "[x]  ";
        return $"{indent}{mark} {Title}";
    }

    /// <summary>
    /// Refuses, because a question has no children.
    /// </summary>
    /// <remarks>
    /// GoF's answer to this, under implementation issue 4: a default that fails is the price
    /// of declaring child management on the Component, and the compensation is that the client
    /// never has to test a type. Note that this refusal is a <em>run-time</em> failure — the
    /// call compiles.
    /// </remarks>
    public void Add(IFormElement element) =>
        throw new NotSupportedException($"a question has no children: {Title}");
}
