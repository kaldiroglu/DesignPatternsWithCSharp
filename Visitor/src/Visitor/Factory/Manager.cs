namespace dev.kaldiroglu.Visitor.Factory;

public class Manager : Employee
{
    public const int ManagementPayment = 3000;

    protected string departmentManaged;

    public Manager(int no, string name, int year, string workingDepartment, string departmentManaged)
        : base(no, name, year, workingDepartment)
    {
        this.departmentManaged = departmentManaged;
    }

    public override void Work()
    {
        Console.WriteLine("Manager is working!");
        Manage();
    }

    public virtual void Manage()
    {
        Console.WriteLine("Manager manages department: " + departmentManaged);
    }

    public override double CalculateSalary()
    {
        return base.CalculateSalary() + ManagementPayment;
    }

    public override void PrintInfo()
    {
        Console.WriteLine("\nManager Info");
        base.PrintInfo();
        Console.WriteLine("Managing Department: " + departmentManaged);
    }
}
