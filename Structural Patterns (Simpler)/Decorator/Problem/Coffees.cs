namespace DevKaldiroglu.DP.Structural.Decorator.Problem;

public abstract class Coffee
{
    public abstract string Description();
    public abstract decimal Cost();
}

public class SimpleCoffee : Coffee
{
    public override string Description() => "Coffee";
    public override decimal Cost() => 5.00m;
}

public class CoffeeWithMilk : Coffee
{
    public override string Description() => "Coffee, milk";
    public override decimal Cost() => 5.00m + 1.00m;
}

public class CoffeeWithSugar : Coffee
{
    public override string Description() => "Coffee, sugar";
    public override decimal Cost() => 5.00m + 0.50m;
}

public class CoffeeWithMilkAndSugar : Coffee
{
    public override string Description() => "Coffee, milk, sugar";
    public override decimal Cost() => 5.00m + 1.00m + 0.50m;
}

public static class ProblemDemo
{
    public static void Run()
    {
        Coffee[] menu = { new SimpleCoffee(), new CoffeeWithMilk(), new CoffeeWithSugar(), new CoffeeWithMilkAndSugar() };
        foreach (var c in menu) Console.WriteLine($"{c.Description()} — ${c.Cost()}");
    }
}
