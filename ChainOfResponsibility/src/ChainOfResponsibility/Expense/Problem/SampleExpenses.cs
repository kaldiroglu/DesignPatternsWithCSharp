using System.Globalization;

namespace dev.kaldiroglu.ChainOfResponsibility.Expense.Problem;

/// <summary>The six expenses that every stage runs on.</summary>
internal static class SampleExpenses
{
    internal static List<Expense> All() =>
    [
        new Expense("Emre", 800, "books"),
        new Expense("Emre", 5_000, "a laptop"),
        new Expense("Burak", 5_000, "a conference"),
        new Expense("Emre", 30_000, "a server"),
        new Expense("Cem", 30_000, "a training course"),
        new Expense("Emre", 500_000, "a new office")
    ];

    internal static string Header(Expense expense) =>
        expense.Submitter + ", " + expense.Amount.ToString(CultureInfo.InvariantCulture) + ", " + expense.Purpose;
}
