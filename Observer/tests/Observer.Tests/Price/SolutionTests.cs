namespace dev.kaldiroglu.Observer.Tests.Price
{
    using System.Reflection;
    using global::dev.kaldiroglu.Observer.Price.Solution;
    using Xunit;
    using PriceMain = global::dev.kaldiroglu.Observer.Price.Solution.Main;

    /// <summary>
    /// The price feed with listeners. Every figure the Part 3 slides quote about it is
    /// asserted here.
    /// </summary>
    public class SolutionTests
    {
        // With listeners the chart shows 102, 106, 100, 103, and the alert at 105 fires.
        [Fact]
        public void ListenersSeeEveryChange()
        {
            var feed = new PriceFeed("ACME", 100);
            var chart = new Chart();
            var ticker = new Ticker();
            var alert = new PriceAlert(105);
            feed.Subscribe(chart);
            feed.Subscribe(ticker);
            feed.Subscribe(alert);

            foreach (int price in new[] { 102, 106, 100, 103 })
            {
                feed.SetPrice(price);
            }

            Assert.Equal(new[] { 102, 106, 100, 103 }, chart.Points);
            Assert.Equal(4, chart.Points.Count); // told about all four changes
            Assert.True(alert.Fired);
            Assert.Equal("ACME 103 up", ticker.Shown);
        }

        // A lambda can listen, and it logs every change from the old price to the new one.
        // Java checks that PriceListener is a @FunctionalInterface with one method. C# has no
        // such annotation: the closest check is that IPriceListener is an interface that
        // declares one method, and the lambda goes through the Subscribe(Action) overload.
        [Fact]
        public void ALambdaListens()
        {
            Assert.True(typeof(IPriceListener).IsInterface);
            Assert.Single(typeof(IPriceListener).GetMethods(
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));

            var feed = new PriceFeed("ACME", 100);
            var log = new List<string>();
            feed.Subscribe(change => log.Add(change.OldPrice + "->" + change.NewPrice));
            foreach (int price in new[] { 102, 106, 100, 103 })
            {
                feed.SetPrice(price);
            }

            Assert.Equal(new[] { "100->102", "102->106", "106->100", "100->103" }, log);
        }

        // An unchanged price tells nobody.
        [Fact]
        public void NoChangeNoNotification()
        {
            var feed = new PriceFeed("ACME", 100);
            var chart = new Chart();
            feed.Subscribe(chart);

            feed.SetPrice(100);

            Assert.Empty(chart.Points);
        }

        // A listener can stop listening while the program runs.
        [Fact]
        public void Unsubscribe()
        {
            var feed = new PriceFeed("ACME", 100);
            var chart = new Chart();
            feed.Subscribe(chart);
            feed.SetPrice(101);
            feed.Unsubscribe(chart);
            feed.SetPrice(102);

            Assert.Equal(new[] { 101 }, chart.Points);
        }

        /// <summary>A listener that stops listening the first time it is told.</summary>
        private sealed class ListenOnce(PriceFeed feed) : IPriceListener
        {
            public void PriceChanged(PriceChange change) => feed.Unsubscribe(this);
        }

        // A listener that unsubscribes while it is being told does not break the loop.
        [Fact]
        public void UnsubscribeDuringNotification()
        {
            var feed = new PriceFeed("ACME", 100);
            var chart = new Chart();
            feed.Subscribe(new ListenOnce(feed));
            feed.Subscribe(chart);

            var thrown = Record.Exception(() => feed.SetPrice(101));
            Assert.Null(thrown);
            feed.SetPrice(102);
            Assert.Equal(new[] { 101, 102 }, chart.Points);
        }

        // The price is set before the listeners are told.
        [Fact]
        public void TheSubjectIsConsistentWhenItNotifies()
        {
            var feed = new PriceFeed("ACME", 100);
            var seen = new List<int>();
            feed.Subscribe(change => seen.Add(feed.Price));

            feed.SetPrice(104);

            Assert.Equal(new[] { 104 }, seen);
        }

        // The feed knows only IPriceListener, and no listener reads the feed.
        [Fact]
        public void TheFeedKnowsOnlyTheInterface()
        {
            var feedFields = Printed.DeclaredFieldTypes(typeof(PriceFeed));
            Assert.DoesNotContain(typeof(Chart), feedFields);
            Assert.DoesNotContain(typeof(Ticker), feedFields);
            Assert.DoesNotContain(typeof(PriceAlert), feedFields);

            foreach (var listener in new[] { typeof(Chart), typeof(Ticker), typeof(PriceAlert) })
            {
                Assert.True(typeof(IPriceListener).IsAssignableFrom(listener), listener.Name);
                Assert.DoesNotContain(typeof(PriceFeed), Printed.DeclaredFieldTypes(listener));
            }
        }

        // Main prints 100 reads and no alert for polling, and every change and the alert for listeners.
        [Fact]
        public void MainOutput()
        {
            var lines = Printed.By(PriceMain.Run);

            Assert.Equal(new[]
            {
                "Polling:   chart [102, 100, 103], alert fired: false, reads 100",
                "Listeners: chart [102, 106, 100, 103], alert fired: true, ticker ACME 103 up, "
                    + "lambda [100->102, 102->106, 106->100, 100->103]",
            }, lines);
        }
    }
}
