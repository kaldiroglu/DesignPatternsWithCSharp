namespace dev.kaldiroglu.Observer.Publisher;

/// <summary>
/// Two people and a bank subscribe to Newsweek, and one person also to FourFourTwo. Then a
/// new Newsweek comes out.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Observer.Demo -- publisher</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        Publisher publisher = new Publisher();
        IPublication newsweek = publisher.Newsweek;
        IPublication fourFourTwo = publisher.FourFourTwo;

        ISubscriber akin = new IndividualSubscriber("Akin");
        newsweek.AddSubscriber(akin);
        fourFourTwo.AddSubscriber(akin);

        ISubscriber sevgi = new IndividualSubscriber("Sevgi");
        newsweek.AddSubscriber(sevgi);

        ISubscriber bank1 = new InstitutionalSubscriber("BankOne");
        newsweek.AddSubscriber(bank1);

        publisher.PublishNewsweek();
    }
}
