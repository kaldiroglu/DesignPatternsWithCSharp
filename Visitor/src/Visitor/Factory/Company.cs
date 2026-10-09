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

    /// <summary>Applies the visitor to every employee.</summary>
    public void Accept(IVisitor hv)
    {
        foreach (Employee employee in employees)
            employee.Accept(hv);
    }
}
