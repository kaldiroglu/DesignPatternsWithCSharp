namespace dev.kaldiroglu.TemplateMethod.Hw.CallCenter;

/// <summary>One call center. In a real system the two methods would call its web service.</summary>
public sealed class IstanbulCallCenter : CallImport
{
    protected override IReadOnlyList<CallRecord> RetrieveMetadata()
    {
        return [new CallRecord("IST-1", 120), new CallRecord("IST-2", 45)];
    }

    protected override Recording RetrieveAudio(CallRecord record)
    {
        return new Recording(record.Id, record.Seconds);
    }
}
