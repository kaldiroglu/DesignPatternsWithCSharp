namespace dev.kaldiroglu.Observer.Tests.Publisher
{
    using global::dev.kaldiroglu.Observer.Publisher;
    using Xunit;

    /// <summary>Magazines and their subscribers: one publish, two different reactions.</summary>
    public class PublisherTests
    {
        // Akin and Sevgi read the new issue, and BankOne puts it on the shelf.
        [Fact]
        public void OnePublishTwoReactions()
        {
            IPublication newsweek = new Newsweek("Newsweek");
            newsweek.AddSubscriber(new IndividualSubscriber("Akin"));
            newsweek.AddSubscriber(new IndividualSubscriber("Sevgi"));
            newsweek.AddSubscriber(new InstitutionalSubscriber("BankOne"));

            var lines = Printed.By(() => newsweek.Publish("2026-10-09"));

            Assert.Equal(new[]
            {
                "Akin is reading Newsweek - 2026-10-09",
                "Sevgi is reading Newsweek - 2026-10-09",
                "Newsweek - 2026-10-09 is on the shelf of BankOne",
            }, lines);
        }

        // A person can subscribe to both magazines, and a removed subscriber is not told.
        [Fact]
        public void TwoMagazines()
        {
            var publisher = new global::dev.kaldiroglu.Observer.Publisher.Publisher();
            ISubscriber akin = new IndividualSubscriber("Akin");
            ISubscriber bank = new InstitutionalSubscriber("BankOne");
            publisher.Newsweek.AddSubscriber(akin);
            publisher.FourFourTwo.AddSubscriber(akin);
            publisher.FourFourTwo.AddSubscriber(bank);
            publisher.FourFourTwo.RemoveSubscriber(bank);

            Assert.Equal(new[] { "Akin is reading FourFourTwo - May" },
                Printed.By(() => publisher.FourFourTwo.Publish("May")));
            Assert.Equal("Newsweek", publisher.Newsweek.Name);
        }

        // Test prints that Akin and Sevgi are reading the issue, and that it is on the shelf of BankOne.
        [Fact]
        public void TestOutput()
        {
            var lines = Printed.By(Test.Run);

            Assert.Equal(5, lines.Count);
            Assert.Equal(" New Newsweek On The Way", lines[1]);
            Assert.StartsWith("Akin is reading Newsweek - ", lines[2]);
            Assert.StartsWith("Sevgi is reading Newsweek - ", lines[3]);
            Assert.EndsWith(" is on the shelf of BankOne", lines[4]);
        }
    }
}
