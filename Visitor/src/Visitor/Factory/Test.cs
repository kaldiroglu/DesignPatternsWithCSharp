namespace dev.kaldiroglu.Visitor.Factory;

/// <summary>
/// A company of five random employees gets a health check, and then the boss gets one.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. <see cref="HR"/> picks the employees at random, so the
/// output changes from run to run. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- factory</c>.
/// </remarks>
public static class Test
{
    public static void Run()
    {
        Company company = new Company(5);
        company.PopulateCompany();

        HealthVisitor hv = new HealthVisitor();

        company.SetVisitor(hv);

        Boss boss = new Boss("Memet Emmi", 52);
        boss.Accept(hv);
    }
}
