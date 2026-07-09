namespace DevKaldiroglu.DP.Structural.Composite.Solution;

public interface IOrgUnit
{
    string Name { get; }
    int Headcount { get; }
    decimal TotalSalaryCost { get; }
    Employee? FindEmployee(string id);
}

public sealed class Employee : IOrgUnit
{
    public string Id { get; }
    public string Name { get; }
    public decimal AnnualSalary { get; }

    public Employee(string id, string name, decimal annualSalary)
    {
        Id = id; Name = name; AnnualSalary = annualSalary;
    }

    public int Headcount => 1;
    public decimal TotalSalaryCost => AnnualSalary;
    public Employee? FindEmployee(string id) => Id == id ? this : null;

    public override string ToString() => $"Employee{{{Id}, {Name}}}";
}

public sealed class Department : IOrgUnit
{
    private readonly List<IOrgUnit> _children = new();

    public string Name { get; }
    public Department(string name) => Name = name;

    public Department Add(IOrgUnit child)
    {
        if (ReferenceEquals(child, this)) throw new ArgumentException("self-reference");
        _children.Add(child);
        return this;
    }

    public int Headcount => Walk().OfType<Employee>().Count();
    public decimal TotalSalaryCost => Walk().OfType<Employee>().Sum(e => e.AnnualSalary);

    public Employee? FindEmployee(string id) =>
        Walk().OfType<Employee>().FirstOrDefault(e => e.Id == id);

    /// <summary>Iterative DFS with identity-based cycle protection.</summary>
    private IEnumerable<IOrgUnit> Walk()
    {
        var visited = new HashSet<IOrgUnit>(ReferenceEqualityComparer.Instance);
        var stack = new Stack<IOrgUnit>();
        stack.Push(this);
        while (stack.Count > 0)
        {
            var cur = stack.Pop();
            if (!visited.Add(cur)) continue;
            yield return cur;
            if (cur is Department d)
            {
                for (int i = d._children.Count - 1; i >= 0; i--) stack.Push(d._children[i]);
            }
        }
    }
}

public static class SolutionDemo
{
    public static void Run()
    {
        IOrgUnit company = new Department("Acme")
            .Add(new Department("Engineering")
                .Add(new Department("Platform")
                    .Add(new Employee("e1", "Ada",  180000m))
                    .Add(new Employee("e2", "Brad", 160000m)))
                .Add(new Employee("e3", "Cleo", 210000m)))
            .Add(new Employee("e4", "Dax", 250000m));

        Console.WriteLine($"Total cost: ${company.TotalSalaryCost}");
        Console.WriteLine($"Headcount:  {company.Headcount}");
        Console.WriteLine($"Find e2:    {company.FindEmployee("e2")?.ToString() ?? "null"}");
        Console.WriteLine($"Find none:  {company.FindEmployee("nope")?.ToString() ?? "null"}");
    }
}
