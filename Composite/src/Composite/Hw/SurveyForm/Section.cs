namespace dev.kaldiroglu.Composite.Hw.SurveyForm;

/// <summary>The Composite: a titled group of questions and other sections.</summary>
public sealed class Section(string title) : IFormElement
{
    private readonly List<IFormElement> _children = [];

    public string Title { get; } = title ?? throw new ArgumentNullException(nameof(title));

    /// <summary>Concatenates rather than sums — the shape most real composite operations take.</summary>
    public IReadOnlyList<string> Validate()
    {
        var problems = new List<string>();
        foreach (var child in _children)
        {
            problems.AddRange(child.Validate());
        }

        return problems.AsReadOnly();
    }

    public int QuestionCount() => _children.Sum(c => c.QuestionCount());

    public int AnsweredCount() => _children.Sum(c => c.AnsweredCount());

    public string Render(string indent)
    {
        var output = new System.Text.StringBuilder(indent + Title);
        foreach (var child in _children)
        {
            output.AppendLine().Append(child.Render(indent + "    "));
        }

        return output.ToString();
    }

    public void Add(IFormElement element)
    {
        if (ReferenceEquals(element, this))
        {
            throw new ArgumentException("a section cannot contain itself", nameof(element));
        }

        _children.Add(element);
    }

    public Section With(params IFormElement[] elements)
    {
        foreach (var element in elements)
        {
            Add(element);
        }

        return this;
    }
}
