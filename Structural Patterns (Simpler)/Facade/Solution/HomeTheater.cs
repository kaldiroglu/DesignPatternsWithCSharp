namespace DevKaldiroglu.DP.Structural.Facade.Solution;

public class Amplifier
{
    public void On()  => Console.WriteLine("Amplifier on");
    public void Off() => Console.WriteLine("Amplifier off");
    public void SetSurroundSound() => Console.WriteLine("Amplifier surround mode");
    public void SetVolume(int level) => Console.WriteLine($"Amplifier volume: {level}");
}

public class DvdPlayer
{
    public void On()  => Console.WriteLine("DVD player on");
    public void Off() => Console.WriteLine("DVD player off");
    public void Play(string movie) => Console.WriteLine($"DVD playing: {movie}");
    public void Stop() => Console.WriteLine("DVD stopped");
}

public class Projector
{
    public void On()  => Console.WriteLine("Projector on");
    public void Off() => Console.WriteLine("Projector off");
    public void WideScreenMode() => Console.WriteLine("Projector wide screen mode");
}

public class Screen
{
    public void Down() => Console.WriteLine("Screen lowered");
    public void Up()   => Console.WriteLine("Screen raised");
}

public class Lights
{
    public void Dim(int level) => Console.WriteLine($"Lights dimmed to {level}%");
    public void On() => Console.WriteLine("Lights on");
}

public class HomeTheaterFacade
{
    private readonly Amplifier _amp;
    private readonly DvdPlayer _dvd;
    private readonly Projector _projector;
    private readonly Screen _screen;
    private readonly Lights _lights;

    public HomeTheaterFacade(Amplifier amp, DvdPlayer dvd, Projector projector, Screen screen, Lights lights)
    {
        _amp = amp; _dvd = dvd; _projector = projector; _screen = screen; _lights = lights;
    }

    public void WatchMovie(string movie)
    {
        _lights.Dim(10);
        _screen.Down();
        _projector.On();
        _projector.WideScreenMode();
        _amp.On();
        _amp.SetSurroundSound();
        _amp.SetVolume(5);
        _dvd.On();
        _dvd.Play(movie);
    }

    public void EndMovie()
    {
        _dvd.Stop();
        _dvd.Off();
        _amp.Off();
        _projector.Off();
        _screen.Up();
        _lights.On();
    }
}

public static class SolutionDemo
{
    public static void Run()
    {
        var theater = new HomeTheaterFacade(
            new Amplifier(), new DvdPlayer(), new Projector(), new Screen(), new Lights());
        theater.WatchMovie("Inception");
        Console.WriteLine("--- movie over ---");
        theater.EndMovie();
    }
}
