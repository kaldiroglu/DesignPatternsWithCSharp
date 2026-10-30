namespace dev.kaldiroglu.Visitor.Factory;

public class Director : Manager
{
    protected int bonus;

    public Director(int no, string name, int year, string workingDepartment, string managingDepartment, int bonus)
        : base(no, name, year, workingDepartment, managingDepartment)
    {
        this.bonus = bonus;
    }

    public override void Work()
    {
        Console.WriteLine("Director is working!");
        Manage();
    }

    public override void Manage()
    {
        Console.WriteLine("Director manages whole company!");
        MakeAStrategicPlan();
    }

    public void MakeAStrategicPlan()
    {
        Console.WriteLine("Director makes a strategic plan for the company!");
    }

    public override int CalculateSalary()
    {
        return base.CalculateSalary() + ManagementPayment + bonus;
    }

    public override void PrintInfo()
    {
        Console.WriteLine("\nDirector Info");
        base.PrintInfo();
    }
}
