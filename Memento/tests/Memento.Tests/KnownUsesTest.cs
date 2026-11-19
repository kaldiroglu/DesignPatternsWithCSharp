namespace dev.kaldiroglu.Memento.Tests;

using System.ComponentModel;
using System.Data;
using Xunit;

/// <summary>The .NET known uses on the Part 4 slide: IEditableObject, DataRow, and a record copy.</summary>
public class KnownUsesTest
{
    /// <summary>A row that saves its name on BeginEdit and goes back to it on CancelEdit.</summary>
    private sealed class Customer : IEditableObject
    {
        private string? saved;

        public string Name { get; set; } = "Elif";

        public void BeginEdit() => saved = Name;

        public void CancelEdit()
        {
            if (saved is not null)
            {
                Name = saved;
            }
        }

        public void EndEdit() => saved = null;
    }

    /// <summary>BeginEdit saves, CancelEdit goes back.</summary>
    [Fact]
    public void AnEditableObjectGoesBack()
    {
        Customer customer = new Customer();
        customer.BeginEdit();
        customer.Name = "Burak";
        customer.CancelEdit();
        Assert.Equal("Elif", customer.Name);
    }

    /// <summary>A DataRow keeps its original values, and RejectChanges goes back to them.</summary>
    [Fact]
    public void ADataRowRejectsChanges()
    {
        DataTable table = new DataTable();
        table.Columns.Add("Name", typeof(string));
        DataRow row = table.Rows.Add("Elif");
        table.AcceptChanges();
        row["Name"] = "Burak";
        row.RejectChanges();
        Assert.Equal("Elif", row["Name"]);
    }

    private sealed record Snapshot(string Place, List<string> Items);

    /// <summary>A record copy made with <c>with</c> is shallow: it shares the list.</summary>
    [Fact]
    public void ARecordCopySharesItsList()
    {
        Snapshot original = new Snapshot("bridge", new List<string> { "sword" });
        Snapshot copy = original with { Place = "cave" };
        original.Items.Add("potion");
        Assert.Equal(new[] { "sword", "potion" }, copy.Items);
        Assert.Equal("cave", copy.Place);
    }
}
