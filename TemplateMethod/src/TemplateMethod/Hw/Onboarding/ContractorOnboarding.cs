namespace dev.kaldiroglu.TemplateMethod.Hw.Onboarding;

/// <summary>A contractor gets e-mail only, and brings their own laptop.</summary>
public sealed class ContractorOnboarding : Onboarding
{
    protected override IReadOnlyList<string> Accounts(string person)
    {
        return ["e-mail for " + person];
    }

    protected override void HandOverEquipment(string person)
    {
        // a contractor brings their own laptop
    }
}
