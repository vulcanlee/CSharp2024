namespace mauiPrismNavigationEventCycle.Services;

public class CurrentLogSnapshotService
{
    public List<string> CurrentLogs { get; set; }

    object lockObject = new object();
    int keepLogsCount = 2000;
    public CurrentLogSnapshotService()
    {
        CurrentLogs = new List<string>();
    }

    public void AddLog(string log, Action<string> logHandler)
    {
        lock (lockObject)
        {
            log = MakeLogMessage(log);
            CurrentLogs.Insert(0, log);
            logHandler?.Invoke(log);

            if (CurrentLogs.Count > keepLogsCount)
            {
                var needRemoveItems = CurrentLogs.Count - keepLogsCount;
                for (int i = 0; i < needRemoveItems; i++)
                {
                    CurrentLogs.Remove(CurrentLogs.Last());
                }
            }
        }

    }

    string MakeLogMessage(string log)
    {
        return $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}{Environment.NewLine}{log}";
    }
}
