namespace dev.kaldiroglu.State.Hw.Document;

/// <summary>
/// Homework 2: the transitions in one central place.
/// <para>
/// The original deck describes two ways to manage transitions: the states decide (as in
/// <c>Order.Solution</c>), or a central object decides. This is the second way. The whole
/// life of a document is one table, so a reviewer can read every rule on one screen, and a
/// new rule is one line.
/// </para>
/// <para>
/// The cost: the statuses are only names. A status that needs its own behavior or data —
/// like <c>Order.Solution.Shipped</c> — does not fit a table.
/// </para>
/// </summary>
public sealed class Workflow
{
    private readonly Dictionary<Status, Dictionary<Action, Status>> next = [];

    public Workflow()
    {
        Allow(Status.DRAFT, Action.SUBMIT, Status.IN_REVIEW);
        Allow(Status.IN_REVIEW, Action.APPROVE, Status.PUBLISHED);
        Allow(Status.IN_REVIEW, Action.REJECT, Status.DRAFT);
        Allow(Status.PUBLISHED, Action.ARCHIVE, Status.ARCHIVED);
    }

    private void Allow(Status from, Action action, Status to)
    {
        if (!next.TryGetValue(from, out var actions))
        {
            actions = [];
            next[from] = actions;
        }
        actions[action] = to;
    }

    /// <summary>The status after <paramref name="action"/>, or <c>null</c> when the action is not allowed.</summary>
    public Status? After(Status from, Action action) =>
        next.TryGetValue(from, out var actions) && actions.TryGetValue(action, out var to) ? to : null;
}
