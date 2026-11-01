namespace dev.kaldiroglu.CreationalIntroduction.Employee.Better;

public class Compensation
{
    public decimal Salary { get; }

    public Compensation(decimal? salary)
    {
        Salary = salary ?? 0m;
    }

    public override bool Equals(object? o)
    {
        if (ReferenceEquals(this, o)) return true;
        if (o is not Compensation that) return false;
        return Salary == that.Salary;
    }

    public override int GetHashCode() => Salary.GetHashCode();
}
