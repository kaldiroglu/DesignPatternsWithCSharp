namespace DevKaldiroglu.DP.Structural.Adapter.Solution;

public interface IMediaPlayer
{
    void Play(string type, string fileName);
}

public interface IAdvancedMediaPlayer
{
    void PlayVlc(string fileName);
    void PlayMp4(string fileName);
}

public class VlcPlayer : IAdvancedMediaPlayer
{
    public void PlayVlc(string fileName) => Console.WriteLine($"Playing vlc file: {fileName}");
    public void PlayMp4(string fileName) { /* not supported */ }
}

public class Mp4Player : IAdvancedMediaPlayer
{
    public void PlayVlc(string fileName) { /* not supported */ }
    public void PlayMp4(string fileName) => Console.WriteLine($"Playing mp4 file: {fileName}");
}

public class MediaAdapter : IMediaPlayer
{
    private readonly IAdvancedMediaPlayer _advanced;

    public MediaAdapter(string type)
    {
        _advanced = type.ToLowerInvariant() switch
        {
            "vlc" => new VlcPlayer(),
            "mp4" => new Mp4Player(),
            _ => throw new ArgumentException($"Unsupported advanced format: {type}")
        };
    }

    public void Play(string type, string fileName)
    {
        switch (type.ToLowerInvariant())
        {
            case "vlc": _advanced.PlayVlc(fileName); break;
            case "mp4": _advanced.PlayMp4(fileName); break;
        }
    }
}

public class AudioPlayer : IMediaPlayer
{
    public void Play(string type, string fileName)
    {
        var t = type.ToLowerInvariant();
        if (t == "mp3") Console.WriteLine($"Playing mp3 file: {fileName}");
        else if (t is "vlc" or "mp4") new MediaAdapter(type).Play(type, fileName);
        else Console.WriteLine($"Format not supported: {type}");
    }
}

public static class SolutionDemo
{
    public static void Run()
    {
        IMediaPlayer player = new AudioPlayer();
        player.Play("mp3", "song.mp3");
        player.Play("mp4", "movie.mp4");
        player.Play("vlc", "clip.vlc");
        player.Play("avi", "old.avi");
    }
}
