using System.Reflection;
using Xunit;
using dev.kaldiroglu.CreationalIntroduction.Bank;
using BetterConnection = dev.kaldiroglu.CreationalIntroduction.Db.Better.DatabaseConnection;
using ProblemConnection = dev.kaldiroglu.CreationalIntroduction.Db.Problem.DatabaseConnection;
using BetterEmployee = dev.kaldiroglu.CreationalIntroduction.Employee.Better.Employee;
using ProblemEmployee = dev.kaldiroglu.CreationalIntroduction.Employee.Problem.Employee;
using Compensation = dev.kaldiroglu.CreationalIntroduction.Employee.Better.Compensation;
using ContactInfo = dev.kaldiroglu.CreationalIntroduction.Employee.Better.ContactInfo;
using JobInfo = dev.kaldiroglu.CreationalIntroduction.Employee.Better.JobInfo;
using PersonalInfo = dev.kaldiroglu.CreationalIntroduction.Employee.Better.PersonalInfo;
using WorkInfo = dev.kaldiroglu.CreationalIntroduction.Employee.Better.WorkInfo;

namespace dev.kaldiroglu.CreationalIntroduction.Tests;

/// <summary>The figures and behaviors the Introduction to Creational Patterns deck states.</summary>
public class ChapterFiguresTests
{
    private const BindingFlags Declared =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    private static int InstanceFields(Type type) => type.GetFields(Declared).Length;

    /// <summary>A setter here is a property with a public <c>set</c> or <c>init</c> accessor.</summary>
    private static int Setters(Type type) =>
        type.GetProperties(Declared).Count(p => p.SetMethod is { IsPublic: true });

    /// <summary>A record has the compiler-generated clone method; no other class does.</summary>
    private static bool IsRecord(Type type) =>
        type.GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public) is not null;

    [Fact]
    public void TheProblemEmployeeHasTwentyOneFieldsAndFourConstructors()
    {
        Assert.Equal(21, InstanceFields(typeof(ProblemEmployee)));
        Assert.Equal(4, typeof(ProblemEmployee).GetConstructors().Length);
        Assert.Equal(21, Setters(typeof(ProblemEmployee)));
    }

    [Fact]
    public void TheBetterEmployeeHasFiveParts()
    {
        Assert.Equal(5, InstanceFields(typeof(BetterEmployee)));
        Assert.Equal(5, typeof(BetterEmployee).GetConstructors()[0].GetParameters().Length);
    }

    [Fact]
    public void TheFivePartsHoldTheTwentyOneFields()
    {
        Assert.Equal(6, InstanceFields(typeof(PersonalInfo)));
        Assert.Equal(4, InstanceFields(typeof(ContactInfo)));
        Assert.Equal(7, InstanceFields(typeof(JobInfo)));
        Assert.Equal(3, InstanceFields(typeof(WorkInfo)));
        Assert.Equal(1, InstanceFields(typeof(Compensation)));
    }

    [Fact]
    public void TheBetterEmployeeAlwaysHasACompensation()
    {
        var employee = new BetterEmployee(null, null, null, null, null);
        Assert.Equal(0m, employee.Compensation.Salary);
    }

    [Fact]
    public void BothConnectionsHaveFifteenFields()
    {
        Assert.Equal(15, InstanceFields(typeof(ProblemConnection)));
        Assert.Equal(15, InstanceFields(typeof(BetterConnection)));
    }

    [Fact]
    public void TheBetterConnectionHasNoSettersAndOnlyFinalFields()
    {
        var connection = typeof(BetterConnection);
        Assert.Equal(0, Setters(connection));
        Assert.All(connection.GetFields(Declared), field => Assert.True(field.IsInitOnly, field.Name));
        Assert.Empty(connection.GetConstructors());
    }

    [Fact]
    public void TheFailingFigureFromTheReviewNowPasses()
    {
        // 1000 + 1000 * 0.01 * 36 is 1359.9999999999998 in double arithmetic.
        Assert.Equal(1360.0, new BankLoan(1000, 0.01, 36, 1360).TotalRepayment);
    }

    [Fact]
    public void BankLoanRejectsAWrongTotal()
    {
        Assert.Throws<ArgumentException>(() => new BankLoan(1000, 0.01, 12, 1100));
    }

    [Fact]
    public void SettersAcceptAWrongTotal()
    {
        var loan = new LoanWithSetters();
        loan.Principal = 1000;
        loan.Rate = 0.01;
        loan.Months = 12;
        loan.TotalRepayment = 1100;
        Assert.Equal(1100, loan.TotalRepayment);
        Assert.Equal(1120, loan.CorrectTotal());
    }

    [Fact]
    public void SecureLoanHasNoPublicConstructorAndNoSetters()
    {
        Assert.Empty(typeof(SecureLoan).GetConstructors());
        Assert.Equal(0, Setters(typeof(SecureLoan)));
        Assert.Equal(1120, SecureLoan.Create(1000, 0.01, 12).TotalRepayment);
    }

    [Fact]
    public void TheRecordCalculatesItsTotalAndChecksItsArguments()
    {
        Assert.True(IsRecord(typeof(LoanRecord)));
        Assert.Equal(1120, new LoanRecord(1000, 0.01, 12).TotalRepayment);
        Assert.Throws<ArgumentException>(() => new LoanRecord(1000, 0.01, 0));
    }

    [Fact]
    public void TheCheckRunsBeforeTheBaseConstructor()
    {
        Assert.Equal(6, new ShortTerm(6).Months);
        Assert.Throws<ArgumentException>(() => new ShortTerm(36));
    }
}
