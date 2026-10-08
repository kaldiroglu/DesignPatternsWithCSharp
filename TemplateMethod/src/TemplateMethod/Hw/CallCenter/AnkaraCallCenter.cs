namespace dev.kaldiroglu.TemplateMethod.Hw.CallCenter;

/// <summary>
/// A second call center. Its audio for one call is cut short, so the standard verification
/// step rejects it — the call center's code cannot skip that step.
/// </summary>
public sealed class AnkaraCallCenter : CallImport
{
    protected override IReadOnlyList<CallRecord> RetrieveMetadata()
    {
        return [new CallRecord("ANK-1", 300), new CallRecord("ANK-2", 60)];
    }

    protected override Recording RetrieveAudio(CallRecord record)
    {
        int seconds = record.Id == "ANK-2" ? 30 : record.Seconds;
        return new Recording(record.Id, seconds);
    }
}
