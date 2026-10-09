namespace dev.kaldiroglu.Mediator.Traffic;

/// <summary>
/// The client: a junction, a traffic police officer, and five cars, each on its own thread.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. The cars run on their own threads, so the order of the
/// output changes from run to run. Like the Java, <c>Run()</c> starts the threads and returns
/// without waiting for them; the threads are foreground threads, so the program waits for
/// them before it exits, as the JVM does. Run it with
/// <c>dotnet run --project src/Mediator.Demo -- traffic</c>.
/// </remarks>
public class Test
{
    public static void Run()
    {
        Junction junction = new Junction("Flower");
        ITrafficMediator police = new TrafficPolice("Mehmet", junction);

        Console.WriteLine();

        int numberOfCars = 5;
        Car[] cars = new Car[numberOfCars];

        for (int i = 0; i < numberOfCars; i++)
        {
            Car car = new Car("Car" + i, junction, police, true);
            cars[i] = car;
        }

        Console.WriteLine();

        foreach (Car car in cars)
        {
            car.Start();
        }
    }
}
