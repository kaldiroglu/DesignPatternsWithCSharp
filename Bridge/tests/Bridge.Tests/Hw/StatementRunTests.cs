using dev.kaldiroglu.Bridge.Hw.StatementRun;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Hw;

/// <summary>
/// Homework 1 — the statement run. The screen reader is what forced the Implementor to describe
/// meaning rather than ink.
/// </summary>
public class StatementRunTests
{
    private const string StatementRunNs = "dev.kaldiroglu.Bridge.Hw.StatementRun";

    private static readonly string[][] Lines = [["Consultancy, March", "8 days", "24,000.00"]];

    private static Document InvoiceOn(IMedium medium) =>
        new Invoice(medium, "4417", "Bora Yilmaz", Lines, "25,450.00");

    [Fact(DisplayName = "one invoice class renders onto three media")]
    public void OneAbstractionThreeImplementors()
    {
        Assert.Contains("<h1>Invoice 4417</h1>", InvoiceOn(new HtmlMedium()).Render());
        Assert.Contains("Invoice 4417", InvoiceOn(new PlainTextMedium()).Render());
        Assert.Contains("Document: Invoice 4417.", InvoiceOn(new SpokenMedium()).Render());
    }

    [Fact(DisplayName = "the spoken rendering carries no markup and no layout")]
    public void TheVoiceHasNoPage()
    {
        var spoken = InvoiceOn(new SpokenMedium()).Render();

        Assert.DoesNotContain("<", spoken);
        Assert.DoesNotContain("=", spoken);    // no underlines
        Assert.DoesNotContain("\n", spoken);   // no lines: a voice has none
        Assert.Contains("Amount due of 25,450.00.", spoken);
    }

    [Fact(DisplayName = "every medium can answer every primitive — none of them is about paper")]
    public void NoPrimitiveIsAboutInk()
    {
        string[] primitives = ["Heading", "Field", "Row", "Total", "Output"];
        Assert.Equal(primitives.Length, typeof(IMedium).GetMethods().Length);

        foreach (var method in typeof(IMedium).GetMethods())
        {
            Assert.Contains(method.Name, primitives);
        }
    }

    [Fact(DisplayName = "three documents and three media are six classes, not nine")]
    public void MPlusNNotMTimesN()
    {
        var documents = TypeCensus.ConcreteImplementationsOf(StatementRunNs, typeof(Document));
        var media = TypeCensus.ConcreteImplementationsOf(StatementRunNs, typeof(IMedium));

        Assert.Equal(3, documents);
        Assert.Equal(3, media);
        Assert.Equal(6, documents + media);
        Assert.Equal(9, documents * media);
    }

    [Fact(DisplayName = "a document never learns which medium it has")]
    public void TheAbstractionHoldsOnlyTheInterface() =>
        Assert.Equal(typeof(IMedium), TypeCensus.Field(typeof(Document), "Medium").FieldType);

    [Fact(DisplayName = "a different document reaches the same three media")]
    public void ASecondDocumentCostsOneClass()
    {
        var spoken = new DunningLetter(new SpokenMedium(),
            "Bora Yilmaz", "4417", "25,450.00", 34).Render();
        var html = new AccountStatement(new HtmlMedium(),
            "TR33-0006", "March", Lines, "1,200.00").Render();

        Assert.Contains("Document: Payment reminder.", spoken);
        Assert.Contains("Amount outstanding of 25,450.00.", spoken);
        Assert.Contains("<h1>Account statement</h1>", html);
    }
}
