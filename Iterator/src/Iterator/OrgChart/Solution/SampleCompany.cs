using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Solution;

/// <summary>
/// The small company that the two orders and the change report run on.
/// </summary>
public static class SampleCompany
{
    public static Department Build(string salesHead) => Build(salesHead, "Can", "Elif");

    public static Department Build(string salesHead, string exporter, string supporter)
    {
        Department export = new Department("Export")
            .Add(new Employee(exporter, "export"));
        Department sales = new Department("Sales")
            .Add(new Employee(salesHead, "head of sales"))
            .Add(new Employee("Ali", "sales"))
            .Add(export);
        Department support = new Department("Support")
            .Add(new Employee(supporter, "support"));
        Department operations = new Department("Operations")
            .Add(new Employee("Mert", "head of operations"))
            .Add(support);
        return new Department("Head office")
            .Add(new Employee("Ayse", "CEO"))
            .Add(sales)
            .Add(operations);
    }
}
