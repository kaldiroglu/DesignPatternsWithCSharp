using dev.kaldiroglu.Decorator.Gof.Visual;
using dev.kaldiroglu.Decorator.Gof.Visual.Problem;
using dev.kaldiroglu.Decorator.Gof.Visual.Solution;
using Xunit;
using ProblemTextView = dev.kaldiroglu.Decorator.Gof.Visual.Problem.TextView;
using SolutionTextView = dev.kaldiroglu.Decorator.Gof.Visual.Solution.TextView;

namespace dev.kaldiroglu.Decorator.Tests.Gof.Visual;

/// <summary>
/// The two designs are worth comparing only if they do the same thing. These tests prove
/// they produce identical output, so every remaining difference is a difference of design.
/// </summary>
public class DesignComparisonTests
{
    private const string Text = "hello there";

    [Fact(DisplayName = "subclassing and decorating draw the same picture — scrollbar inside")]
    public void SameOutputScrollbarInside()
    {
        var bySubclassing = ProblemTextView.Render(new BorderedScrolledTextView(5, 2, Text));
        var byDecorating = IVisualComponent.Render(
            new BorderDecorator(new ScrollDecorator(new SolutionTextView(5, 2, Text))));

        Assert.Equal(bySubclassing, byDecorating);
    }

    [Fact(DisplayName = "subclassing and decorating draw the same picture — scrollbar outside")]
    public void SameOutputScrollbarOutside()
    {
        var bySubclassing = ProblemTextView.Render(new ScrolledBorderedTextView(5, 2, Text));
        var byDecorating = IVisualComponent.Render(
            new ScrollDecorator(new BorderDecorator(new SolutionTextView(5, 2, Text))));

        Assert.Equal(bySubclassing, byDecorating);
    }

    [Fact(DisplayName = "the pictures are what the Java port produces, character for character")]
    public void ThePicturesThemselves()
    {
        // The same strings the Java tests assert, so the two ports cannot drift apart.
        // Border(Scroll(text)) — the scrollbar is inside the border, so the border grows
        // around it. This is the left-hand picture on the deck's "what it actually draws".
        Assert.Equal(
            "+------+\n|hello^|\n|therev|\n+------+",
            IVisualComponent.Render(
                new BorderDecorator(new ScrollDecorator(new SolutionTextView(5, 2, Text)))));

        Assert.Equal(
            "+-----+^\n|hello|#\n|there|#\n+-----+v",
            IVisualComponent.Render(
                new ScrollDecorator(new BorderDecorator(new SolutionTextView(5, 2, Text)))));
    }

    [Fact(DisplayName = "GoF Consequence 1: a responsibility can be added twice")]
    public void ABorderTwiceOver()
    {
        var twice = IVisualComponent.Render(
            new BorderDecorator(new BorderDecorator(new SolutionTextView(5, 2, Text))));

        Assert.Equal("+-------+\n|+-----+|\n||hello||\n||there||\n|+-----+|\n+-------+", twice);
    }

    [Fact(DisplayName = "the difference is in what a third embellishment would cost")]
    public void CostOfTheNextEmbellishment()
    {
        // Problem: TextView, BorderedTextView, ScrolledTextView, BorderedScrolledTextView,
        //          ScrolledBorderedTextView = 5 classes for 2 embellishments.
        // Solution: TextView, Decorator, BorderDecorator, ScrollDecorator = 4 classes, and
        //          every combination and order of a third comes free.
        const int problemClassesForTwo = 5;
        const int solutionClassesForTwo = 4;
        const int problemClassesForThree = 16; // 1 + sum over k of 3!/(3-k)! = 1 + 3 + 6 + 6
        const int solutionClassesForThree = 5;

        Assert.Equal(11, problemClassesForThree - problemClassesForTwo);
        Assert.Equal(1, solutionClassesForThree - solutionClassesForTwo);
    }
}
