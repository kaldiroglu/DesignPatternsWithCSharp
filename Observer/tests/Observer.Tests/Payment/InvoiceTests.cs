namespace dev.kaldiroglu.Observer.Tests.Payment
{
    using global::dev.kaldiroglu.Observer.Payment;
    using Xunit;

    /// <summary>
    /// The invoice built on an Observable base class. The Part 3 slide says the boss is added
    /// first and is told second; this class checks it. Java uses the JDK's
    /// <c>java.util.Observable</c>; the C# port uses its own <c>Payment.Observable</c>.
    /// </summary>
    public class InvoiceTests
    {
        // The boss is added first, but the accountant is told first.
        [Fact]
        public void TheAccountantIsToldFirst()
        {
            var invoice = new Invoice(10_000);
            invoice.AddObserver(new Boss());
            invoice.AddObserver(new Accountant());

            var lines = Printed.By(() => invoice.PayBalance(5000));

            var updates = lines.Where(line => line.EndsWith("received an update.")).ToList();
            Assert.Equal(new[] { "Accountant has received an update.", "Boss has received an update." }, updates);
            Assert.Contains(lines, line => line.StartsWith("Invoice [balance=5000.0, no="));
        }

        // Test prints the accountant's update before the boss's, and only the accountant's
        // after the boss is removed.
        [Fact]
        public void TestOutput()
        {
            var lines = Printed.By(Test.Run);

            var updates = lines.Where(line => line.EndsWith("received an update.")).ToList();
            Assert.Equal(new[]
            {
                "Accountant has received an update.", "Boss has received an update.",
                "Accountant has received an update.",
            }, updates);
            Assert.Equal(2, lines.Count(line => line == "Some payment made."));
            Assert.StartsWith("Invoice [balance=3000.0, no=", lines[^1]);
        }

        // Without SetChanged, NotifyObservers tells nobody.
        [Fact]
        public void SetChangedComesFirst()
        {
            var invoice = new Invoice(100);
            invoice.AddObserver(new Accountant());

            Assert.Empty(Printed.By(invoice.NotifyObservers));
        }

        // Observable is a class, so Invoice cannot extend anything else.
        [Fact]
        public void ObservableIsAClass()
        {
            Assert.False(typeof(Observable).IsInterface);
            Assert.Equal(typeof(Observable), typeof(Invoice).BaseType);
        }
    }
}
