namespace DevKaldiroglu.DP.Structural.Composite.Problem;

public class Employee
{
    public string Id { get; }
    public string Name { get; }
    public decimal AnnualSalary { get; }

    public Employee(string id, string name, decimal annualSalary)
    {
        Id = id; Name = name; AnnualSalary = annualSalary;
    }
}

public class Department
{
    public string Name { get; }
    public List<object> Children { get; } = new();

    public Department(string name) => Name = name;

    public void Add(object child)
    {
        if (child is not (Employee or Department)) throw new ArgumentException("Unknown child type");
        Children.Add(child);
    }
}

public class HrReports
{
    public decimal TotalSalaryCost(object node) => node switch
    {
        Employee e => e.AnnualSalary,
        Department d => d.Children.Sum(TotalSalaryCost),
        _ => throw new ArgumentException("Unknown node type")
    };

    public int Headcount(object node) => node switch
    {
        Employee => 1,
        Department d => d.Children.Sum(Headcount),
        _ => throw new ArgumentException("Unknown node type")
    };

    public Employee? FindEmployee(object node, string id)
    {
        if (node is Employee e) return e.Id == id ? e : null;
        if (node is Department d)
        {
            foreach (var child in d.Children)
            {
                var hit = FindEmployee(child, id);
                if (hit is not null) return hit;
            }
            return null;
        }
        throw new ArgumentException("Unknown node type");
    }
}

public static class ProblemDemo
{
    public static void Run()
    {
        var company = new Department("Acme");
        var eng = new Department("Engineering");
        var platform = new Department("Platform");
        platform.Add(new Employee("e1", "Ada",  180000m));
        platform.Add(new Employee("e2", "Brad", 160000m));
        eng.Add(platform);
        eng.Add(new Employee("e3", "Cleo", 210000m));
        company.Add(eng);
        company.Add(new Employee("e4", "Dax", 250000m));

        var hr = new HrReports();
        Console.WriteLine($"Total cost: ${hr.TotalSalaryCost(company)}");
        Console.WriteLine($"Headcount:  {hr.Headcount(company)}");
        Console.WriteLine($"Find e2:    {hr.FindEmployee(company, "e2")?.Name ?? "null"}");
    }
}
