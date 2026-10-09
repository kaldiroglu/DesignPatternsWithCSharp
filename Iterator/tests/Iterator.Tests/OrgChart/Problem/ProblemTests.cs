namespace dev.kaldiroglu.Iterator.Tests.OrgChart.Problem;

using dev.kaldiroglu.Iterator.OrgChart.Domain;
using dev.kaldiroglu.Iterator.OrgChart.Problem;
using Xunit;

/// <summary>
/// The three attempts of Part 1: a department that gives out its lists, one that copies, and
/// one that calls back. Ported from the Java <c>orgchart.problem.ProblemTest</c>.
/// </summary>
public class ProblemTests
{
    private static readonly Employee Ayse = new("Ayse", "CEO");
    private static readonly Employee Deniz = new("Deniz", "head of sales");
    private static readonly Employee Ali = new("Ali", "sales");
    private static readonly Employee Can = new("Can", "export");
    private static readonly Employee Mert = new("Mert", "head of operations");
    private static readonly Employee Elif = new("Elif", "support");

    /// <summary>The same company as <c>OrgChart.Solution.Main</c>, built as stage three.</summary>
    private static CallbackDepartment CallbackCompany(Employee salesHead)
    {
        var export = new CallbackDepartment("Export").Add(Can);
        var sales = new CallbackDepartment("Sales").Add(salesHead).Add(Ali).Add(export);
        var support = new CallbackDepartment("Support").Add(Elif);
        var operations = new CallbackDepartment("Operations").Add(Mert).Add(support);
        return new CallbackDepartment("Head office").Add(Ayse).Add(sales).Add(operations);
    }

    // ------------------------------------------------------------------ stage one

    [Fact(DisplayName = "stage one works: payroll gets one payslip for every person")]
    public void PayrollPaysEveryoneOnce()
    {
        var sales = new OpenDepartment("Sales").Add(Deniz).Add(Ali)
            .Add(new OpenDepartment("Export").Add(Can));
        var company = new OpenDepartment("Head office").Add(Ayse).Add(sales);

        Assert.Equal(["payslip for Ayse", "payslip for Deniz", "payslip for Ali", "payslip for Can"],
            new PayrollRun().Payslips(company));
    }

    [Fact(DisplayName = "stage one: callers get the real lists, so any caller can add or remove people")]
    public void CallersCanChangeTheRealLists()
    {
        var sales = new OpenDepartment("Sales").Add(Deniz).Add(Ali);

        sales.Members.Remove(Ali);

        // A caller removed Ali from the department itself.
        Assert.Equal(["payslip for Deniz"], new PayrollRun().Payslips(sales));
    }

    // ------------------------------------------------------------------ stage two

    [Fact(DisplayName = "stage two: callers get a copy they cannot change, in one order")]
    public void StageTwoGivesAnUnchangeableCopy()
    {
        var sales = new CopyingDepartment("Sales").Add(Deniz).Add(Ali)
            .Add(new CopyingDepartment("Export").Add(Can));
        var company = new CopyingDepartment("Head office").Add(Ayse).Add(sales)
            .Add(new CopyingDepartment("Operations").Add(Mert));

        var everyone = company.Everyone();

        Assert.Equal([Ayse, Deniz, Ali, Can, Mert], everyone);
        // Java's unmodifiable list throws UnsupportedOperationException. The C# list is
        // read-only, and adding through ICollection<T> throws NotSupportedException.
        Assert.Throws<NotSupportedException>(() => ((ICollection<Employee>)everyone).Add(Elif));
        // Every call makes a new copy.
        Assert.NotSame(everyone, company.Everyone());
    }

    // ------------------------------------------------------------------ stage three

    [Fact(DisplayName = "stage three: no copy, and two orders of walking")]
    public void StageThreeWalksInTwoOrders()
    {
        var company = CallbackCompany(Deniz);
        var byDepartment = new List<Employee>();
        var byLevel = new List<Employee>();

        company.ForEachMember(byDepartment.Add);
        company.ForEachMemberByLevel(byLevel.Add);

        Assert.Equal([Ayse, Deniz, Ali, Can, Mert, Elif], byDepartment);
        Assert.Equal([Ayse, Deniz, Ali, Mert, Can, Elif], byLevel);
    }

    [Fact(DisplayName = "stage three: the change report finds the new head of sales, but only after copying both charts")]
    public void TheProblemReportFindsTheChange()
    {
        var zeynep = new Employee("Zeynep", "head of sales");

        // Java returns Optional<String>; the C# port returns string?, with null for none.
        var change = new ChangeReport().FirstDifference(CallbackCompany(Deniz), CallbackCompany(zeynep));

        Assert.Equal("Deniz (head of sales) -> Zeynep (head of sales)", change);
    }

    [Fact(DisplayName = "stage three: the change report answers none for equal charts and sees a size change")]
    public void TheProblemReportHandlesEqualAndShorterCharts()
    {
        var shorter = new CallbackDepartment("Head office").Add(Ayse);

        Assert.Null(new ChangeReport().FirstDifference(CallbackCompany(Deniz), CallbackCompany(Deniz)));
        Assert.Equal("the charts have different sizes",
            new ChangeReport().FirstDifference(CallbackCompany(Deniz), shorter));
    }
}
