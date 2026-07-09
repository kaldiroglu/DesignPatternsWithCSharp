namespace DevKaldiroglu.DP.Structural.Facade.Problem;

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

public static class ProblemDemo
{
    public static void Run()
    {
        var amp = new Amplifier();
        var dvd = new DvdPlayer();
        var projector = new Projector();
        var screen = new Screen();
        var lights = new Lights();

        lights.Dim(10);
        screen.Down();
        projector.On();
        projector.WideScreenMode();
        amp.On();
        amp.SetSurroundSound();
        amp.SetVolume(5);
        dvd.On();
        dvd.Play("Inception");

        Console.WriteLine("--- movie over ---");

        dvd.Stop();
        dvd.Off();
        amp.Off();
        projector.Off();
        screen.Up();
        lights.On();
    }
}
