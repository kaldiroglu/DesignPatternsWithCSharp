namespace dev.kaldiroglu.Memento.Tests.Hw.Rollback;

using dev.kaldiroglu.Memento.Hw.Rollback;
using Xunit;

/// <summary>Homework 2: three transfers, and the second fails.</summary>
public class BatchTest
{
    /// <summary>When the second of three transfers fails, every account goes back to where it was.</summary>
    [Fact]
    public void AFailedBatchIsRolledBack()
    {
        Account elif = new Account("Elif", 100);
        Account burak = new Account("Burak", 50);
        Account mert = new Account("Mert", 0);
        Batch batch = new Batch();
        batch.Add(elif, burak, 80);
        batch.Add(mert, elif, 30);
        batch.Add(burak, mert, 10);
        Assert.Equal("rolled back: Mert cannot pay 30", batch.Run([elif, burak, mert]));
        Assert.Equal("Elif 100", elif.ToString());
        Assert.Equal("Burak 50", burak.ToString());
        Assert.Equal("Mert 0", mert.ToString());
    }

    /// <summary>A batch where every transfer succeeds is done.</summary>
    [Fact]
    public void ABatchThatSucceeds()
    {
        Account elif = new Account("Elif", 100);
        Account burak = new Account("Burak", 50);
        Batch batch = new Batch();
        batch.Add(elif, burak, 80);
        batch.Add(burak, elif, 30);
        Assert.Equal("done", batch.Run([elif, burak]));
        Assert.Equal("Elif 50", elif.ToString());
        Assert.Equal("Burak 100", burak.ToString());
    }
}
