using dev.kaldiroglu.Decorator.Toast.Problem;
using dev.kaldiroglu.Decorator.Toast.Solution;

namespace dev.kaldiroglu.Decorator.Demo;

/// <summary>The toast shop, one method for each slide's output.</summary>
public static class ToastExamples
{
    /// <summary>Cheese, sausage, tomato, ketchup, salad: the five the solution offers.</summary>
    private const int ToppingCount = 5;

    private static IReadOnlyList<IToast> MenuItems() =>
    [
        new CheeseToast(),
        new CheeseTomatoToast(),
        new SausageToast(),
        new SausageTomatoToast(),
        new CheeseSausageToast()
    ];

    public static void Menu()
    {
        Console.WriteLine("The menu, one class per item:");
        foreach (var toast in MenuItems())
        {
            Console.WriteLine($"  {toast}");
        }
    }

    public static void CannotSell() =>
        Console.WriteLine(
            """
            What the shop cannot sell:
              cheese + ketchup            no class
              sausage + tomato + salad    no class
              tomato added to a sausage toast, at the counter, after ordering
                                          no class, and no way to write one

            Adding a topping is a code change. Adding it to an order already
            placed is impossible: a toast's toppings are fixed by its type, and
            an object cannot change its type after it is constructed.
            """);

    public static void Combinations()
    {
        var menu = MenuItems();

        // A class per combination of n toppings is 2^n - 1 classes, because each topping is
        // independently on or off and the empty toast is the bread.
        var combinations = (1 << ToppingCount) - 1;
        Console.WriteLine($"With {ToppingCount} toppings, a class per combination is "
                          + $"2^{ToppingCount} - 1 = {combinations} classes.");
        Console.WriteLine($"This namespace has {menu.Count}, and one of those five items already needed");
        Console.WriteLine("hand-copied prices because a class cannot have two base classes.");
        Console.WriteLine();

        var ok = menu[0].CalculatePrice() == 8      // cheese
                 && menu[1].CalculatePrice() == 10  // cheese + tomato
                 && menu[2].CalculatePrice() == 8   // sausage
                 && menu[3].CalculatePrice() == 10  // sausage + tomato
                 && menu[4].CalculatePrice() == 11  // cheese + sausage, hand-copied
                 && combinations == 31;
        Console.WriteLine(ok ? "self-check: prices as expected" : "self-check: FAILED");
    }

    public static void StepByStep()
    {
        Console.WriteLine("Building an Ayvalik toast, one topping at a time:");

        IToastable toast = new ToastBread();
        Console.WriteLine($"  bare bread          {toast.CalculatePrice()}");

        toast = new Cheese(toast);
        Console.WriteLine($"  + cheddar cheese    {toast.CalculatePrice()}");

        toast = new Sausage(toast);
        Console.WriteLine($"  + sucuk sausage     {toast.CalculatePrice()}");

        toast = new Tomato(toast);
        Console.WriteLine($"  + tomato            {toast.CalculatePrice()}");

        toast = new Ketchup(toast);
        Console.WriteLine($"  + ketchup           {toast.CalculatePrice()}");

        toast = new Salad(toast);
        Console.WriteLine($"  + Russian salad     {toast.CalculatePrice()}");

        // Every line above assigned to the same variable of the same type.
        PrintToppings(toast);
    }

    public static void OneExpression()
    {
        IToastable toast = FullToast();

        Console.WriteLine($"The whole Ayvalik toast at once: {toast.CalculatePrice()}");
        PrintToppings(toast);
    }

    public static void DoubleCheese()
    {
        IToastable toast = new Cheese(new Cheese(new ToastBread()));

        Console.WriteLine($"Double cheese: {toast.CalculatePrice()}  (bread 5 + cheese 3 + cheese 3)");
        Console.WriteLine("  A decorator wraps a toastable, and a decorated toast is one too,");
        Console.WriteLine("  so nothing stops the same topping being applied again.");
    }

    public static void PromotionPosition()
    {
        IToastable discountOutside = new Promotion(FullToast(), "student discount", 25);
        IToastable discountInside = new Salad(new Ketchup(new Tomato(new Sausage(
            new Cheese(new Promotion(new ToastBread(), "student discount", 25))))));

        Console.WriteLine("A 25% discount, same toast, two positions in the chain:");
        Console.WriteLine($"  outermost, so it discounts everything   {discountOutside.CalculatePrice()}");
        Console.WriteLine($"  innermost, so it discounts only bread   {discountInside.CalculatePrice()}");
        Console.WriteLine("  Order is a design decision, not a detail. The solution makes it");
        Console.WriteLine("  visible and cheap to change; the problem namespace made it neither.");
    }

    private static IToastable FullToast() =>
        new Salad(new Ketchup(new Tomato(new Sausage(new Cheese(new ToastBread())))));

    private static void PrintToppings(IToastable toast)
    {
        foreach (var topping in toast.GetToppings())
        {
            Console.WriteLine($"    {topping}");
        }
    }
}
