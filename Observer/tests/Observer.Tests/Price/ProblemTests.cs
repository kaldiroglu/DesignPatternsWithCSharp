namespace dev.kaldiroglu.Observer.Tests.Price
{
    using global::dev.kaldiroglu.Observer.Price.Problem;
    using Xunit;

    /// <summary>
    /// The three designs of Part 1: the feed calls every screen, the screens poll, and the
    /// screens poll often and act only on a change. Every figure the Part 1 slides quote is
    /// asserted here.
    /// </summary>
    public class ProblemTests
    {
        /// <summary>Ten seconds, ten polls a second. The price spikes to 106 and back between two polls.</summary>
        private static void TenSeconds(PriceFeed feed, ChangeOnlyReaders readers)
        {
            for (int tenth = 1; tenth <= 100; tenth++)
            {
                if (tenth == 20) feed.SetPrice(102);
                if (tenth == 55) { feed.SetPrice(106); feed.SetPrice(100); }
                if (tenth == 80) feed.SetPrice(103);
                readers.Poll();
            }
        }

        // Stage one sees every price, but the feed knows every screen by class.
        [Fact]
        public void TheFeedCallsEveryScreen()
        {
            var chart = new Chart();
            var ticker = new Ticker();
            var alert = new PriceAlert(105);
            var feed = new DirectPriceFeed(chart, ticker, alert);

            foreach (int price in new[] { 102, 106, 100, 103 })
            {
                feed.SetPrice(price);
            }

            Assert.Equal(new[] { 102, 106, 100, 103 }, chart.Points);
            Assert.Equal(103, ticker.Shown);
            Assert.True(alert.Fired);

            // DirectPriceFeed takes its screens in a primary constructor. The compiler stores
            // each captured parameter in a private field of the parameter's type.
            var fieldTypes = Printed.DeclaredFieldTypes(typeof(DirectPriceFeed));
            Assert.Contains(typeof(Chart), fieldTypes);
            Assert.Contains(typeof(Ticker), fieldTypes);
            Assert.Contains(typeof(PriceAlert), fieldTypes);
        }

        // Stage two: the feed knows nobody, and most polls find the same price.
        [Fact]
        public void TheScreensPoll()
        {
            var feed = new PriceFeed(100);
            var chart = new Chart();
            var readers = new PollingReaders(feed, chart, new Ticker(), new PriceAlert(105));

            readers.Poll();
            readers.Poll();
            feed.SetPrice(102);
            readers.Poll();

            // The chart draws the same price again.
            Assert.Equal(new[] { 100, 100, 102 }, chart.Points);
            // The feed holds only numbers.
            Assert.All(Printed.DeclaredFieldTypes(typeof(PriceFeed)), t => Assert.Equal(typeof(int), t));
        }

        // Stages two and three: the reader classes hold every screen, so a new screen is an edit to them.
        [Theory]
        [InlineData(typeof(PollingReaders))]
        [InlineData(typeof(ChangeOnlyReaders))]
        public void ANewScreenIsAnEditToTheReaders(Type readers)
        {
            var fieldTypes = Printed.DeclaredFieldTypes(readers);
            Assert.Contains(typeof(Chart), fieldTypes);
            Assert.Contains(typeof(Ticker), fieldTypes);
            Assert.Contains(typeof(PriceAlert), fieldTypes);
        }

        // Stage three: 100 reads, four changes, three of them seen, and the alert at 105 never fires.
        [Fact]
        public void StageThreeMissesTheSpike()
        {
            var feed = new PriceFeed(100);
            var chart = new Chart();
            var ticker = new Ticker();
            var alert = new PriceAlert(105);
            var readers = new ChangeOnlyReaders(feed, chart, ticker, alert);
            int readsBefore = feed.Reads;

            TenSeconds(feed, readers);

            Assert.Equal(100, feed.Reads - readsBefore);
            Assert.Equal(new[] { 102, 100, 103 }, chart.Points);
            Assert.Equal(3, chart.Points.Count); // three of the four changes are seen
            Assert.DoesNotContain(106, chart.Points);
            Assert.False(alert.Fired);
            Assert.Equal(103, ticker.Shown);
        }

        // Stage three: an unchanged price costs one read and no work.
        [Fact]
        public void AnUnchangedPriceCostsOneRead()
        {
            var feed = new PriceFeed(100);
            var chart = new Chart();
            var readers = new ChangeOnlyReaders(feed, chart, new Ticker(), new PriceAlert(105));
            int readsBefore = feed.Reads;

            readers.Poll();

            Assert.Equal(1, feed.Reads - readsBefore);
            Assert.Empty(chart.Points);
        }

        // StageOneMain prints the screens after each price, and the alert fires on 106.
        [Fact]
        public void StageOneMainOutput()
        {
            var lines = Printed.By(StageOneMain.Run).ToList();

            Assert.Equal(20, lines.Count); // four prices, five lines each
            Assert.Equal("alert fired: false", lines[3]); // 102 is under the limit
            Assert.Equal("alert fired: true", lines[8]); // 106 is over the limit
            Assert.Equal(new[]
            {
                "Stage one, the feed calls every screen:", "chart [102, 106, 100, 103]",
                "ticker shows 103", "alert fired: true", "",
            }, lines.Skip(15));
        }

        // StageThreeMain: the thread polls ten times a second, and only the changes reach the chart.
        [Fact]
        public void StageThreeMainOutput()
        {
            const string header = "Stage three, the screens poll ten times a second:";
            Assert.Equal(new[]
            {
                header, "chart [102]", "ticker shows 102", "alert fired: false", "",
                header, "chart [102, 103]", "ticker shows 103", "alert fired: false", "",
            }, Printed.By(StageThreeMain.Run));
        }

        // StageTwoMain: the thread polls three times, and each screen shows what it got.
        [Fact]
        public void StageTwoMainOutput()
        {
            const string header = "Stage two, the screens poll every second:";
            Assert.Equal(new[]
            {
                header, "chart [100]", "ticker shows 100", "alert fired: false", "",
                header, "chart [100, 100]", "ticker shows 100", "alert fired: false", "",
                header, "chart [100, 100, 102]", "ticker shows 102", "alert fired: false", "",
            }, Printed.By(StageTwoMain.Run));
        }

        // FastSpikeMain prints each screen after each change seen, no alert, and 100 reads.
        [Fact]
        public void FastSpikeMainOutput()
        {
            Assert.Equal(new[]
            {
                "Stage three, ten polls a second, poll 20:", "chart [102]", "ticker shows 102",
                "alert fired: false", "",
                "Stage three, ten polls a second, poll 55:", "chart [102, 100]", "ticker shows 100",
                "alert fired: false", "",
                "Stage three, ten polls a second, poll 80:", "chart [102, 100, 103]", "ticker shows 103",
                "alert fired: false", "",
                "Reads in ten seconds: 100",
            }, Printed.By(FastSpikeMain.Run));
        }
    }
}
