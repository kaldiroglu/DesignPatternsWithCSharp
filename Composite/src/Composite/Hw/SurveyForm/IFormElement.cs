namespace dev.kaldiroglu.Composite.Hw.SurveyForm;

/// <summary>
/// The Component: a part of a form, whether it is one question or a whole section.
/// </summary>
/// <remarks>
/// <para>
/// The operation to notice is <see cref="Validate"/>. It returns a <em>collection</em> rather
/// than a number, which is the more common shape in real systems and slightly harder: a
/// composite has to concatenate what its children return instead of adding it up.
/// </para>
/// <para>
/// This namespace takes the <b>transparent</b> side of GoF's implementation issue 4 —
/// <see cref="Add"/> is declared here, on the Component, so every element looks alike and a
/// client never asks what it is holding. The price is on the next line down.
/// </para>
/// </remarks>
public interface IFormElement
{
    string Title { get; }

    /// <summary>Everything wrong with this element and anything inside it. Empty means valid.</summary>
    IReadOnlyList<string> Validate();

    int QuestionCount();

    int AnsweredCount();

    string Render(string indent);

    /// <summary>
    /// Adds a child.
    /// </summary>
    /// <exception cref="NotSupportedException">
    /// On a <see cref="Question"/>, which has no children. That is the cost of transparency:
    /// the mistake compiles and fails at run time. <c>Composite.Graphic</c> and
    /// <c>Composite.FileSystem</c> make the other choice, so the three can be compared.
    /// </exception>
    void Add(IFormElement element);
}
