using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.CreationalIntroduction.Tests;

/// <summary>
/// Every runner entry named by a slide of this chapter: it must return normally and print
/// the lines the slide quotes.
/// </summary>
public class SlideRunsTests
{
    [Theory]
    [InlineData("employee-problem-setters",
        "Alex Kim | Intern | id=null | salary=null",
        "null null | Full-time | id=null | salary=null")]
    [InlineData("employee-problem-constructors",
        "null null | Full-time | id=null | salary=null",
        "Sarah Johnson | Full-time | id=null | salary=null",
        "Michael Chen | Full-time | id=EXE2020045 | salary=180000")]
    [InlineData("employee-problem-swapped-arguments",
        "Email     : Engineering",
        "Department: sarah.johnson@company.com")]
    [InlineData("employee-better",
        "intern: Alex Kim | Data Science | Data Analyst Intern | Intern | remote=false | tz=PST | salary=25000",
        "remoteContractor: Elena Rodriguez | Marketing | UI/UX Designer | Contract | remote=true | tz=PST | salary=0")]
    [InlineData("db-problem",
        "localhost:3306/myapp_dev user=dev_user ssl=true",
        "localhost:3306/null user=dev_user ssl=true")]
    [InlineData("db-better",
        "prod-db-cluster.company.com:5432/myapp_production user=prod_user ssl=true",
        "Rejected: database must not be blank")]
    [InlineData("bank-setters",
        "Total stored : 1100.0",
        "Correct total: 1120.0")]
    [InlineData("bank-constructor-check",
        "Accepted: 1120.0",
        "Rejected: Math doesn't add up!")]
    [InlineData("bank-secure-loan",
        "Total: 1120.0",
        "Rejected: Principal must be positive")]
    [InlineData("bank-loan-record",
        "LoanRecord[principal=1000.0, rate=0.01, months=12]",
        "Total: 1120.0",
        "Rejected: Term must be at least 1 month")]
    [InlineData("bank-short-term",
        "Months: 6",
        "Rejected: A short term is at most 12 months")]
    public void PrintsWhatTheSlideQuotes(string entry, params string[] lines)
    {
        var printed = Printed.By("CreationalIntroduction.Demo", entry);

        foreach (var line in lines)
        {
            Assert.Contains(line, printed);
        }
    }
}

/// <summary>Runs a demo program in this process and returns what it printed.</summary>
internal static class Printed
{
    public static string By(string assembly, params string[] args)
    {
        var original = Console.Out;
        var buffer = new StringWriter();
        Console.SetOut(buffer);
        try
        {
            Assembly.Load(assembly).EntryPoint!.Invoke(null, [args]);
        }
        finally
        {
            Console.SetOut(original);
        }

        return buffer.ToString().Replace("\r\n", "\n");
    }
}
