using System.Diagnostics;

namespace csNlogTraceId;

public class PerformanceLogger : IDisposable
{
    private readonly Stopwatch _stopwatch;
    private readonly string _taskName;
    private readonly ILogger _logger;

    public PerformanceLogger(ILogger _logger, string taskName)
    {
        _taskName = taskName;
       this._logger = _logger;
        _stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("{TaskName} 開始執行", _taskName);
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        _logger.LogInformation("{TaskName} 結束執行, Execution time: {ElapsedMilliseconds} ms",
            _taskName, _stopwatch.ElapsedMilliseconds);
    }
}