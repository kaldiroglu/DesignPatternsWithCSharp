namespace dev.kaldiroglu.CreationalIntroduction.Employee.Better;

public class WorkInfo
{
    public string WorkLocation { get; set; }
    public bool IsRemoteWorker { get; set; }
    public string TimeZone { get; set; }

    public WorkInfo(string? workLocation, bool isRemoteWorker, string? timeZone)
    {
        WorkLocation = workLocation ?? "";
        IsRemoteWorker = isRemoteWorker;
        TimeZone = timeZone ?? "EST";
    }

    public static WorkInfo Remote(string? timeZone) => new WorkInfo("", true, timeZone);
}
