namespace DevKaldiroglu.DP.Structural.Adapter.Problem;

public class AudioPlayer
{
    public void Play(string type, string fileName)
    {
        if (string.Equals(type, "mp3", StringComparison.OrdinalIgnoreCase))
            Console.WriteLine($"Playing mp3 file: {fileName}");
        else if (string.Equals(type, "mp4", StringComparison.OrdinalIgnoreCase))
            Console.WriteLine($"Playing mp4 file (hard-coded branch): {fileName}");
        else if (string.Equals(type, "vlc", StringComparison.OrdinalIgnoreCase))
            Console.WriteLine($"Playing vlc file (hard-coded branch): {fileName}");
        else
            Console.WriteLine($"Format not supported: {type}");
    }
}

public static class ProblemDemo
{
    public static void Run()
    {
        var player = new AudioPlayer();
        player.Play("mp3", "song.mp3");
        player.Play("mp4", "movie.mp4");
        player.Play("vlc", "clip.vlc");
        player.Play("avi", "old.avi");
    }
}
