namespace dev.kaldiroglu.TemplateMethod.Hw.CallCenter;

/// <summary>The audio of one call, with the length the audio really has.</summary>
public sealed record Recording(string CallId, int Seconds);
