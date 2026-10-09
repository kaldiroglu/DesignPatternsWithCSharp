using System.Globalization;

namespace dev.kaldiroglu.Visitor.Hw.Expression;

/// <summary>Runs three visitors over one expression: it is printed, evaluated and measured.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- hw-expression</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        IExpr expression = new Mul(new Add(new Num(2), new Num(3)), new Neg(new Num(4)));
        Console.WriteLine("Expression: " + expression.Accept(new Printer()));
        Console.WriteLine("Value:      " + expression.Accept(new Evaluator()).ToString(CultureInfo.InvariantCulture));
        Console.WriteLine("Depth:      " + expression.Accept(new DepthCounter()));
    }
}
