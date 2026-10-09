namespace dev.kaldiroglu.Visitor.Factory;

public class Secretary : Employee
{
    protected Manager? managerServed;

    // NOTE: the constructor never assigns managerServed, so it stays null and Serve() prints
    // "null". The Java has the same behavior; it is kept so that both print the same.
    public Secretary(int no, string name, int year, string department, Manager managerServed)
        : base(no, name, year, department)
    {
    }

    public void Serve()
    {
        // Java prints a null reference as "null"; C# would print an empty string.
        Console.WriteLine("Secretary " + Name + " serves her manager: " + (managerServed?.ToString() ?? "null"));
    }
}
