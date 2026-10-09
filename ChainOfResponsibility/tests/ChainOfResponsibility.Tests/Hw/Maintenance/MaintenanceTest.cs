namespace dev.kaldiroglu.ChainOfResponsibility.Tests.Hw.Maintenance;

using dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;
using Xunit;

/// <summary>Homework 1: the maintenance queue knows only the first developer.</summary>
public class MaintenanceTest
{
    /// <summary>Each request goes to the developer suited for it, and a 30-day improvement reaches the project team.</summary>
    [Fact]
    public void EachRequestFindsItsDeveloper()
    {
        Developer first = new BugFixer("Bora");
        first.Then(new UiDesigner("Ece"))
            .Then(new FeatureDeveloper("Fatih"))
            .Then(new ProjectTeam("the project team"));
        RequestQueue queue = new RequestQueue(first);
        queue.Add(new Request(RequestKind.BUG, "login fails", 1));
        queue.Add(new Request(RequestKind.UI_CHANGE, "new logo", 2));
        queue.Add(new Request(RequestKind.IMPROVEMENT, "export to PDF", 10));
        queue.Add(new Request(RequestKind.IMPROVEMENT, "new reports", 30));
        queue.Add(new Request(RequestKind.PROJECT, "mobile app", 120));

        Assert.Equal(new[]
        {
            "login fails -> Bora",
            "new logo -> Ece",
            "export to PDF -> Fatih",
            "new reports -> the project team",
            "mobile app -> the project team"
        }, queue.ProcessAll());
    }

    /// <summary>A request nobody takes goes back to the team lead.</summary>
    [Fact]
    public void NobodyTakesIt()
    {
        Developer first = new BugFixer("Bora");
        Assert.Equal("new logo -> nobody: back to the team lead",
            first.Take(new Request(RequestKind.UI_CHANGE, "new logo", 2)));
    }
}
