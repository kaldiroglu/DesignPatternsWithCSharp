using System.Reflection;
using dev.kaldiroglu.Decorator.Gof.Visual;
using dev.kaldiroglu.Decorator.Gof.Visual.SkinAndGuts;
using dev.kaldiroglu.Decorator.Gof.Visual.Solution;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Gof.Visual;

/// <summary>
/// GoF implementation issue 4, "changing the skin of an object versus changing its guts"
/// (p. 180), on the book's own border example. Both designs draw the same borders. What
/// differs is what a fourth style costs.
/// </summary>
public class SkinAndGutsTests
{
    private static TextView View() => new(5, 1, "hello");

    [Fact(DisplayName = "the two designs draw exactly the same thing, for every style")]
    public void BothDesignsAgree()
    {
        Assert.Equal(
            IVisualComponent.Render(new SwitchingBorderDecorator(
                View(), SwitchingBorderDecorator.Style.Solid)),
            IVisualComponent.Render(new StyledBorderDecorator(View(), new SolidBorder())));

        Assert.Equal(
            IVisualComponent.Render(new SwitchingBorderDecorator(
                View(), SwitchingBorderDecorator.Style.Dashed)),
            IVisualComponent.Render(new StyledBorderDecorator(View(), new DashedBorder())));

        Assert.Equal(
            IVisualComponent.Render(new SwitchingBorderDecorator(
                View(), SwitchingBorderDecorator.Style.Thick)),
            IVisualComponent.Render(new StyledBorderDecorator(View(), new ThickBorder())));
    }

    [Fact(DisplayName = "the three styles really are different pictures")]
    public void TheStylesDiffer()
    {
        var solid = IVisualComponent.Render(new StyledBorderDecorator(View(), new SolidBorder()));
        var dashed = IVisualComponent.Render(new StyledBorderDecorator(View(), new DashedBorder()));
        var thick = IVisualComponent.Render(new StyledBorderDecorator(View(), new ThickBorder()));

        Assert.Equal("+-----+\n|hello|\n+-----+", solid);
        Assert.Equal("+- - -+\n|hello|\n+- - -+", dashed);
        Assert.Equal("#######\n#hello#\n#######", thick);

        Assert.Equal(3, new[] { solid, dashed, thick }.Distinct().Count());
    }

    [Fact(DisplayName = "a fourth style is a new class: StyledBorderDecorator is not touched")]
    public void ANewStyleNeedsNoEditToTheDecorator()
    {
        // Written here, in the test, without changing one line of the main source tree.
        var dotted = new DottedBorder();

        Assert.Equal(".......\n.hello.\n.......",
            IVisualComponent.Render(new StyledBorderDecorator(View(), dotted)));

        // The switching design has no equivalent: its vocabulary is closed at three, and a
        // fourth means editing the enum and the branch that reads it.
        Assert.Equal(3, Enum.GetValues<SwitchingBorderDecorator.Style>().Length);
    }

    private sealed class DottedBorder : IBorderStyle
    {
        public void Stroke(Canvas canvas, int x, int y, int width, int height)
        {
            for (var i = 0; i < width; i++)
            {
                canvas.Put(x + i, y, '.');
                canvas.Put(x + i, y + height - 1, '.');
            }

            for (var i = 0; i < height; i++)
            {
                canvas.Put(x, y + i, '.');
                canvas.Put(x + width - 1, y + i, '.');
            }
        }
    }

    [Fact(DisplayName = "StyledBorderDecorator names no concrete style; the switching one names its own")]
    public void TheDecoratorDependsOnlyOnTheAbstraction()
    {
        var styled = ReferencedTypes(typeof(StyledBorderDecorator)).ToList();

        Assert.DoesNotContain(typeof(SolidBorder), styled);
        Assert.DoesNotContain(typeof(DashedBorder), styled);
        Assert.DoesNotContain(typeof(ThickBorder), styled);
        Assert.Contains(typeof(IBorderStyle), styled);

        Assert.Contains(typeof(SwitchingBorderDecorator.Style),
            ReferencedTypes(typeof(SwitchingBorderDecorator)));
    }

    /// <summary>Field and parameter types — enough to see what a class is coupled to.</summary>
    private static IEnumerable<Type> ReferencedTypes(Type type) =>
        type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Select(f => f.FieldType)
            .Concat(type.GetConstructors().SelectMany(c => c.GetParameters()).Select(p => p.ParameterType))
            .Concat(type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly
                                    | BindingFlags.Public | BindingFlags.NonPublic)
                .SelectMany(m => m.GetParameters()).Select(p => p.ParameterType));

    [Fact(DisplayName = "the component is still unaware: the skin half is unchanged")]
    public void TheWrappedComponentKnowsNothing()
    {
        var offending = typeof(TextView)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Select(f => f.FieldType.Namespace ?? "")
            .Concat(typeof(TextView).GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly
                                                | BindingFlags.Public)
                .Select(m => m.ReturnType.Namespace ?? ""))
            .Where(ns => ns.EndsWith("SkinAndGuts", StringComparison.Ordinal));

        Assert.Empty(offending);
    }
}
