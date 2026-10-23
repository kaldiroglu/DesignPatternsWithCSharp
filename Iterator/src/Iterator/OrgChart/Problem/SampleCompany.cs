using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// The small company that stage three and the change report run on, built from
/// <see cref="CallbackDepartment"/>s.
/// </summary>
public static class SampleCompany
{
    public static CallbackDepartment Build(string salesHead) => Build(salesHead, "Can", "Elif");

    public static CallbackDepartment Build(string salesHead, string exporter, string supporter)
    {
        var sales = new CallbackDepartment("Sales")
            .Add(new Employee(salesHead, "head of sales"))
            .Add(new Employee("Ali", "sales"))
            .Add(new CallbackDepartment("Export").Add(new Employee(exporter, "export")));
        var operations = new CallbackDepartment("Operations")
            .Add(new Employee("Mert", "head of operations"))
            .Add(new CallbackDepartment("Support").Add(new Employee(supporter, "support")));
        return new CallbackDepartment("Head office")
            .Add(new Employee("Ayse", "CEO")).Add(sales).Add(operations);
    }
}
