namespace dev.kaldiroglu.Visitor.Factory;

public class Employee
{
    public const int BaseSalary = 500;

    public Employee(int no, string name, int year, string department)
    {
        No = no;
        Name = name;
        Year = year;
        Department = department;
    }

    public int No { get; set; }

    public string Name { get; set; }

    public int Year { get; set; }

    public string Department { get; set; }

    public virtual void Work()
    {
        Console.WriteLine("Employee is working!");
    }

    public virtual double CalculateSalary()
    {
        return Year * BaseSalary;
    }

    public virtual void PrintInfo()
    {
        Console.WriteLine("\nNo: " + No);
        Console.WriteLine("Name:" + Name);
        Console.WriteLine("Year: " + Year);
        Console.WriteLine("Department: " + Department);
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
