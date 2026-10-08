using dev.kaldiroglu.Iterator.OrgChart.Domain;

namespace dev.kaldiroglu.Iterator.OrgChart.Problem;

/// <summary>
/// One caller of <see cref="OpenDepartment"/>. It needs everyone in the department, so it
/// writes the recursion itself. The phone book, the roll call and every other caller write the
/// same recursion again.
/// </summary>
public sealed class PayrollRun
{
    public IReadOnlyList<string> Payslips(OpenDepartment department)
    {
        var payslips = new List<string>();
        Collect(department, payslips);
        return payslips;
    }

    private static void Collect(OpenDepartment department, List<string> payslips)
    {
        foreach (Employee employee in department.Members)
        {
            payslips.Add("payslip for " + employee.Name);
        }

        foreach (OpenDepartment unit in department.Units)
        {
            Collect(unit, payslips);
        }
    }
}
