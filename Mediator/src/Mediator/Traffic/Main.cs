namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The client: a junction, a traffic police officer, and five cars, each on its own thread.
/// </summary>
/// <remarks>
/// The cars run on their own threads, so the order of the output changes from run to run.
/// <c>Run()</c> waits for every car to pass. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- traffic</c>.
/// </remarks>
public class Main
{
    public static void Run()
    {
        Junction junction = new Junction("Flower");
        ITrafficMediator police = new TrafficPolice("Mehmet", junction);

        Console.WriteLine();

        int numberOfCars = 5;
        Thread[] cars = new Thread[numberOfCars];

        for (int i = 0; i < numberOfCars; i++)
        {
            cars[i] = new Thread(new Car("Car" + i, police).Run);
        }

        foreach (Thread car in cars)
        {
            car.Start();
        }

        foreach (Thread car in cars)
        {
            car.Join();
        }
    }
}
