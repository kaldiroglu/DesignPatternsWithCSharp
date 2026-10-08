namespace dev.kaldiroglu.TemplateMethod.Hw.Onboarding;

/// <summary>An employee gets e-mail and payroll accounts, and the default equipment.</summary>
public sealed class EmployeeOnboarding : Onboarding
{
    protected override IReadOnlyList<string> Accounts(string person)
    {
        return ["e-mail for " + person, "payroll for " + person];
    }
}
