using System.Globalization;

namespace dev.kaldiroglu.Visitor.Factory;

/// <summary>
/// A <b>ConcreteVisitor</b>: checks everyone who has worked more than five years, every
/// manager's psychological state, and the boss if the boss is over fifty. There is one
/// <c>Visit(Employee)</c> for four classes, so it tests for <see cref="Manager"/> inside it.
/// </summary>
public class HealthVisitor : IVisitor
{
    public void Visit(Employee employee)
    {
        if (employee is Manager)
            CheckPsychologicalStatus((Manager)employee);

        int year = employee.Year;
        if (year > 5)
            CheckHealthStatus(employee);
    }

    public void Visit(Boss boss)
    {
        if (boss.Age > 50)
            CheckHealthStatus(boss);
    }

    private void CheckHealthStatus(Employee employee)
    {
        Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Checking the health status of employee: {employee.No} {employee.Name}"));
    }

    private void CheckPsychologicalStatus(Manager employee)
    {
        Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
                $"Checking the psychological status of manager: {employee.No} {employee.Name}"));
    }

    private void CheckHealthStatus(Boss boss)
    {
        Console.WriteLine("Checking the health status of boss: " + boss.Name);
    }
}
