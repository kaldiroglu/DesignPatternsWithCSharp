namespace dev.kaldiroglu.State.Hw.Document;

/// <summary>The context. It asks the central <see cref="Workflow"/> for its next status.</summary>
public sealed class Document
{
    private readonly Workflow workflow;
    private Status status = Status.DRAFT;

    public Document(Workflow workflow)
    {
        this.workflow = workflow;
    }

    public void Apply(Action action)
    {
        status = workflow.After(status, action) ?? throw new InvalidOperationException(
            "cannot " + action.ToString().ToLowerInvariant() + " a "
            + status.ToString().ToLowerInvariant() + " document");
    }

    public Status Status => status;
}
