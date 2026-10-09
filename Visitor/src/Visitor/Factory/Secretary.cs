namespace dev.kaldiroglu.Visitor.Factory;

public class Secretary : Employee
{
    protected Manager managerServed;

    public Secretary(int no, string name, int year, string department, Manager managerServed)
        : base(no, name, year, department)
    {
        this.managerServed = managerServed;
    }

    public void Serve()
    {
        Console.WriteLine("Secretary " + Name + " serves her manager: " + managerServed.Name);
    }
}
