namespace dev.kaldiroglu.TemplateMethod.Hw.CallCenter;

/// <summary>
/// Homework 1: bring a call center's call records into the company's own system.
/// <para>
/// The five steps are fixed: retrieve the metadata, store it, retrieve the audio, verify the
/// audio against the metadata, store the audio. Only the two "retrieve" steps depend on the
/// call center, so only they are abstract. <see cref="Run"/> is the template method. In Java
/// it is <c>final</c>; here it is a public method without <c>virtual</c>. Either way, no call
/// center can skip the verification.
/// </para>
/// </summary>
public abstract class CallImport
{
    private readonly List<string> stored = [];
    private readonly List<string> rejected = [];

    /// <summary>The template method. Not <c>virtual</c>, so no subclass can override it.</summary>
    public void Run()
    {
        IReadOnlyList<CallRecord> records = RetrieveMetadata();
        foreach (CallRecord record in records)
        {
            stored.Add("metadata " + record.Id);
        }
        foreach (CallRecord record in records)
        {
            Recording? audio = RetrieveAudio(record);
            if (Verify(record, audio))
            {
                stored.Add("audio " + record.Id);
            }
            else
            {
                rejected.Add(record.Id);
            }
        }
    }

    /// <summary>Depends on the call center.</summary>
    protected abstract IReadOnlyList<CallRecord> RetrieveMetadata();

    /// <summary>Depends on the call center.</summary>
    protected abstract Recording? RetrieveAudio(CallRecord record);

    /// <summary>Standard for every call center: the audio must be the call it claims to be.</summary>
    private static bool Verify(CallRecord record, Recording? audio)
    {
        return audio != null && audio.CallId == record.Id
            && audio.Seconds == record.Seconds;
    }

    public IReadOnlyList<string> Stored => stored.ToList().AsReadOnly();

    public IReadOnlyList<string> Rejected => rejected.ToList().AsReadOnly();
}
