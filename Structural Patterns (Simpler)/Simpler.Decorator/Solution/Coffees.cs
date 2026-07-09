namespace DevKaldiroglu.DP.Structural.Decorator.Solution;

public interface ICoffee
{
    string Description();
    decimal Cost();
}

public class SimpleCoffee : ICoffee
{
    public string Description() => "Coffee";
    public decimal Cost() => 5.00m;
}

public abstract class CoffeeDecorator : ICoffee
{
    protected readonly ICoffee Wrapped;
    protected CoffeeDecorator(ICoffee wrapped) => Wrapped = wrapped;
    public virtual string Description() => Wrapped.Description();
    public virtual decimal Cost() => Wrapped.Cost();
}

public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(ICoffee wrapped) : base(wrapped) { }
    public override string Description() => Wrapped.Description() + ", milk";
    public override decimal Cost() => Wrapped.Cost() + 1.00m;
}

public class SugarDecorator : CoffeeDecorator
{
    public SugarDecorator(ICoffee wrapped) : base(wrapped) { }
    public override string Description() => Wrapped.Description() + ", sugar";
    public override decimal Cost() => Wrapped.Cost() + 0.50m;
}

public class VanillaDecorator : CoffeeDecorator
{
    public VanillaDecorator(ICoffee wrapped) : base(wrapped) { }
    public override string Description() => Wrapped.Description() + ", vanilla";
    public override decimal Cost() => Wrapped.Cost() + 0.75m;
}

public static class SolutionDemo
{
    public static void Run()
    {
        ICoffee[] orders = {
            new SimpleCoffee(),
            new MilkDecorator(new SimpleCoffee()),
            new SugarDecorator(new MilkDecorator(new SimpleCoffee())),
            new VanillaDecorator(new SugarDecorator(new MilkDecorator(new SimpleCoffee())))
        };
        foreach (var c in orders) Console.WriteLine($"{c.Description()} — ${c.Cost()}");
    }
}
