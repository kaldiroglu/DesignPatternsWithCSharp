namespace dev.kaldiroglu.ChainOfResponsibility.Expense.ListChain;

using Expense = dev.kaldiroglu.ChainOfResponsibility.Expense.Solution.Expense;

/// <summary>
/// The chain kept as a list. The order of the list is the order of the chain, and the loop
/// in <see cref="Handle"/> asks each handler in turn. No handler knows another one.
/// <para>
/// Each handler still decides by its own rule. That is the difference from
/// <c>Problem.SkippingLimitTable</c>, whose loop walks names and makes every decision itself.
/// </para>
/// </summary>
public sealed class ApprovalChain
{
    private readonly List<ILink> links = [];

    /// <summary>Adds a handler at the end of the chain and returns this chain.</summary>
    public ApprovalChain Then(ILink link)
    {
        links.Add(link);
        return this;
    }

    public string Handle(Expense expense)
    {
        foreach (ILink link in links)
        {
            string? answer = link.TryToHandle(expense);
            if (answer != null)
            {
                return answer;
            }
        }
        return "no one may approve it";
    }
}
