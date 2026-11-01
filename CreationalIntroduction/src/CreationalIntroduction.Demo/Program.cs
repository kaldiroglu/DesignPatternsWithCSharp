// Several example namespaces have a runnable class with the same name (Main, SettersMain).
// Aliases name them apart.
using EmployeeSetters = dev.kaldiroglu.CreationalIntroduction.Employee.Problem.SettersMain;
using EmployeeConstructors = dev.kaldiroglu.CreationalIntroduction.Employee.Problem.ConstructorsMain;
using EmployeeSwappedArguments = dev.kaldiroglu.CreationalIntroduction.Employee.Problem.SwappedArgumentsMain;
using EmployeeBetterMain = dev.kaldiroglu.CreationalIntroduction.Employee.Better.Main;
using DbProblemMain = dev.kaldiroglu.CreationalIntroduction.Db.Problem.Main;
using DbBetterMain = dev.kaldiroglu.CreationalIntroduction.Db.Better.Main;
using BankSetters = dev.kaldiroglu.CreationalIntroduction.Bank.SettersMain;
using BankConstructorCheck = dev.kaldiroglu.CreationalIntroduction.Bank.ConstructorCheckMain;
using BankSecureLoan = dev.kaldiroglu.CreationalIntroduction.Bank.SecureLoanMain;
using BankLoanRecord = dev.kaldiroglu.CreationalIntroduction.Bank.LoanRecordMain;
using BankShortTerm = dev.kaldiroglu.CreationalIntroduction.Bank.ShortTermMain;

namespace dev.kaldiroglu.CreationalIntroduction.Demo;

/// <summary>
/// Runs the examples of the chapter before the creational patterns.
/// <para>
/// Each example runs on its own — <c>dotnet run -- bank-secure-loan</c> — and with no argument
/// all of them run in the order the course presents them.
/// </para>
/// </summary>
public static class Program
{
    private static readonly Dictionary<string, (string Group, Action Run)> Examples = new()
    {
        ["employee-problem-setters"] = ("AN EMPLOYEE WITH 21 FIELDS", EmployeeSetters.Run),
        ["employee-problem-constructors"] = ("AN EMPLOYEE WITH 21 FIELDS", EmployeeConstructors.Run),
        ["employee-problem-swapped-arguments"] = ("AN EMPLOYEE WITH 21 FIELDS", EmployeeSwappedArguments.Run),
        ["employee-better"] = ("AN EMPLOYEE WITH 21 FIELDS", EmployeeBetterMain.Run),
        ["db-problem"] = ("A DATABASE CONNECTION WITH 15 FIELDS", DbProblemMain.Run),
        ["db-better"] = ("A DATABASE CONNECTION WITH 15 FIELDS", DbBetterMain.Run),
        ["bank-setters"] = ("ONE LOAN, ONE INVARIANT", BankSetters.Run),
        ["bank-constructor-check"] = ("ONE LOAN, ONE INVARIANT", BankConstructorCheck.Run),
        ["bank-secure-loan"] = ("ONE LOAN, ONE INVARIANT", BankSecureLoan.Run),
        ["bank-loan-record"] = ("ONE LOAN, ONE INVARIANT", BankLoanRecord.Run),
        ["bank-short-term"] = ("ONE LOAN, ONE INVARIANT", BankShortTerm.Run)
    };

    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            var name = args[0].ToLowerInvariant();
            if (!Examples.TryGetValue(name, out var example))
            {
                Console.WriteLine($"unknown example '{name}'. One of: {string.Join(", ", Examples.Keys)}");
                return;
            }

            example.Run();
            return;
        }

        string? lastGroup = null;
        foreach (var (name, (group, run)) in Examples)
        {
            if (group != lastGroup)
            {
                Heading(group);
                lastGroup = group;
            }

            Section(name);
            run();
        }
    }

    private static void Heading(string title)
    {
        Console.WriteLine("\n" + new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void Section(string title) =>
        Console.WriteLine($"\n--- {title} {new string('-', Math.Max(0, 68 - title.Length))}");
}
