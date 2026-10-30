namespace dev.kaldiroglu.Observer.Tests.Payment
{
    using global::dev.kaldiroglu.Observer.Payment;
    using Xunit;

    /// <summary>
    /// The invoice built on .NET's own <see cref="IObservable{T}"/> and
    /// <see cref="IObserver{T}"/>. The boss subscribes first and is told first; this class
    /// checks it.
    /// </summary>
    public class InvoiceTests
    {
        // The boss subscribes first and is told first.
        [Fact]
        public void TheBossIsToldFirst()
        {
            var invoice = new Invoice(10_000);
            invoice.Subscribe(new Boss());
            invoice.Subscribe(new Accountant());

            var lines = Printed.By(() => invoice.PayBalance(5000));

            var updates = lines.Where(line => line.EndsWith("received an update.")).ToList();
            Assert.Equal(new[] { "Boss has received an update.", "Accountant has received an update." }, updates);
            Assert.Contains(lines, line => line.StartsWith("Invoice [balance=5000.0, no="));
        }

        // Main prints the boss's update before the accountant's, and only the accountant's
        // after the boss is removed.
        [Fact]
        public void TestOutput()
        {
            var lines = Printed.By(Main.Run);

            var updates = lines.Where(line => line.EndsWith("received an update.")).ToList();
            Assert.Equal(new[]
            {
                "Boss has received an update.", "Accountant has received an update.",
                "Accountant has received an update.",
            }, updates);
            Assert.Equal(2, lines.Count(line => line == "Some payment is made."));
            Assert.StartsWith("Invoice: Invoice [balance=3000.0, no=", lines[^1]);
        }

        // Disposing the subscription stops the updates.
        [Fact]
        public void DisposeEndsTheSubscription()
        {
            var invoice = new Invoice(100);
            invoice.Subscribe(new Accountant()).Dispose();

            var lines = Printed.By(() => invoice.PayBalance(10));

            Assert.Equal(new[] { "Some payment is made." }, lines);
        }

        // An observer that subscribes twice is told once.
        [Fact]
        public void ASecondSubscribeIsIgnored()
        {
            var invoice = new Invoice(100);
            var accountant = new Accountant();
            invoice.Subscribe(accountant);
            invoice.Subscribe(accountant);

            var lines = Printed.By(() => invoice.PayBalance(10));

            Assert.Single(lines, line => line.EndsWith("received an update."));
        }

        // The example uses the interfaces of the System namespace, so Invoice keeps its base class free.
        [Fact]
        public void TheInterfacesAreTheOnesInSystem()
        {
            Assert.True(typeof(IObservable<Invoice>).IsAssignableFrom(typeof(Invoice)));
            Assert.True(typeof(IObserver<Invoice>).IsAssignableFrom(typeof(Boss)));
            Assert.True(typeof(IObserver<Invoice>).IsAssignableFrom(typeof(Accountant)));
            Assert.Equal(typeof(object), typeof(Invoice).BaseType);
        }
    }
}
