namespace dev.kaldiroglu.Introduction.Problems.ComplexIfs;

/// <summary>
/// The user that <see cref="UserProcessor.ProcessUser"/> decides about: fourteen values and
/// no way to set them. The properties are <c>virtual</c>, so a demo or a test can give one of
/// them a value through a small subclass.
/// </summary>
public class User
{
    public virtual int Age { get; }
    public virtual int Experience { get; }
    public virtual List<string>? Certifications { get; }
    public virtual int LastLoginDays { get; }
    public virtual int FailedLogins { get; }
    public virtual bool BackgroundCheck { get; }
    public virtual bool ComplianceTraining { get; }
    public virtual bool FinancialClearance { get; }
    public virtual int AuditScore { get; }
    public virtual bool GeneralTraining { get; }
    public virtual int AccountAge { get; }
    public virtual int TrialDaysRemaining { get; }
    public virtual string? SuspensionReason { get; }
    public virtual bool ParentalConsent { get; }
}
