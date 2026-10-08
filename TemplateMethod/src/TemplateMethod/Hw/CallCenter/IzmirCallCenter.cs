namespace dev.kaldiroglu.TemplateMethod.Hw.CallCenter;

/// <summary>The third call center. It has no calls today.</summary>
public sealed class IzmirCallCenter : CallImport
{
    protected override IReadOnlyList<CallRecord> RetrieveMetadata()
    {
        return [];
    }

    protected override Recording RetrieveAudio(CallRecord record)
    {
        return new Recording(record.Id, record.Seconds);
    }
}
