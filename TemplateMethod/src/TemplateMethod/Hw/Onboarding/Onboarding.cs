namespace dev.kaldiroglu.TemplateMethod.Hw.Onboarding;

/// <summary>
/// Homework 2: the first day of a new person.
/// <para>
/// Create the accounts, hand over equipment, assign a mentor, send the welcome message.
/// Employees and contractors differ in two places. Accounts are a primitive operation:
/// each kind must say which ones. Equipment is a <b>hook</b>: by default a laptop is given,
/// and a contractor, who brings their own, overrides it to give nothing.
/// </para>
/// <para>
/// The homework question was whether "skip the equipment" should be a flag or a hook. A
/// flag puts the contractor's rule in this class; a hook keeps it in the contractor's.
/// </para>
/// </summary>
public abstract class Onboarding
{
    private readonly List<string> steps = [];

    /// <summary>
    /// The template method. Not <c>virtual</c>, so no subclass can override it (Java's
    /// <c>final</c>).
    /// </summary>
    public IReadOnlyList<string> Start(string person)
    {
        steps.Clear();
        steps.AddRange(Accounts(person));
        HandOverEquipment(person);
        steps.Add("mentor for " + person);
        steps.Add("welcome message to " + person);
        return steps.ToList().AsReadOnly();
    }

    /// <summary>A primitive operation: which accounts this kind of person gets.</summary>
    protected abstract IReadOnlyList<string> Accounts(string person);

    /// <summary>A hook: give a laptop.</summary>
    protected virtual void HandOverEquipment(string person)
    {
        steps.Add("laptop for " + person);
    }
}
