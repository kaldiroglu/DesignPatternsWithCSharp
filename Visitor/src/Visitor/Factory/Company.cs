namespace dev.kaldiroglu.Visitor.Factory;

/// <summary>The <b>ObjectStructure</b>: the company sends the visitor to every employee.</summary>
public class Company
{
    private readonly int numberOfEmployees;
    private readonly List<Employee> employees;

    private readonly HR hr = new();

    public Company(int numberOfEmployees)
    {
        this.numberOfEmployees = numberOfEmployees;
        employees = new List<Employee>(numberOfEmployees);
    }

    public void PopulateCompany()
    {
        for (int i = 0; i < numberOfEmployees; i++)
        {
            employees.Add(hr.GetAnEmployee());
        }
    }

    // NOTE: the name says "set", but the method applies the visitor to every employee at
    // once and keeps nothing. The Java has the same name and the same behavior.
    public void SetVisitor(IVisitor hv)
    {
        foreach (Employee employee in employees)
            employee.Accept(hv);
    }
}
