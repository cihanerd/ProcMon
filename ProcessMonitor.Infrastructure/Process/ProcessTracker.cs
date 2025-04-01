using ProcessMonitor.Domain;
using System.Diagnostics;

namespace ProcessMonitor.Infrastructure;

public class ProcessTrackerOptions
{
    public List<string> ImportantProcessNames { get; set; } = new List<string>();
}

public class ProcessTracker : IDisposable
{
    private Dictionary<int, (Process Process, DateTime LastCheck, TimeSpan LastTotalProcessorTime)> _trackedProcesses;
    private readonly List<string> _importantProcessNames;

    public ProcessTracker()
    {
        _trackedProcesses = new Dictionary<int, (Process, DateTime, TimeSpan)>();

        _importantProcessNames =  new List<string> {
                    "chrome", "firefox", "iexplore", "msedge",
                    "svchost", "explorer", "devenv", "sqlservr",
                    "w3wp", "iis", "node", "dotnet"
            };

        RefreshProcessList();
    }

    public List<ProcessInfo> GetProcessInfo()
    {
        RefreshProcessList();

        var result = new List<ProcessInfo>();

        foreach (var pair in _trackedProcesses)
        {
            var process = pair.Value.Process;
            if (!IsProcessRunning(process))
                continue;

            try
            {
                var currentTotalProcessorTime = process.TotalProcessorTime;
                var lastTotalProcessorTime = pair.Value.LastTotalProcessorTime;
                var timeDifference = (DateTime.Now - pair.Value.LastCheck).TotalMilliseconds / 1000;

                var cpuUsage = 0.0;
                if (timeDifference > 0)
                {
                    var cpuUsedMs = (currentTotalProcessorTime - lastTotalProcessorTime).TotalMilliseconds;
                    cpuUsage = (cpuUsedMs / (Environment.ProcessorCount * timeDifference * 1000)) * 100;
                }

                _trackedProcesses[pair.Key] = (process, DateTime.Now, currentTotalProcessorTime);

                var processInfo = new ProcessInfo
                {
                    Id = process.Id,
                    Name = process.ProcessName,
                    CpuUsage = Math.Round(cpuUsage, 2),
                    MemoryUsage = process.WorkingSet64,
                    Status = process.Responding ? "Responding" : "Not Responding",
                    StartTime = process.StartTime,
                    TotalProcessorTime = process.TotalProcessorTime,
                    ThreadCount = process.Threads.Count,
                    IsImportant = IsImportantProcess(process.ProcessName)
                };

                result.Add(processInfo);
            }
            catch (Exception)
            {
                continue;
            }
        }

        return result;
    }

    private void RefreshProcessList()
    {
        var currentProcesses = Process.GetProcesses();
        var currentProcessIds = currentProcesses.Select(p => p.Id).ToHashSet();

        var removedProcessIds = _trackedProcesses.Keys.Where(id => !currentProcessIds.Contains(id)).ToList();
        foreach (var id in removedProcessIds)
        {
            _trackedProcesses[id].Process.Dispose();
            _trackedProcesses.Remove(id);
        }

        foreach (var process in currentProcesses)
        {
            if (!_trackedProcesses.ContainsKey(process.Id))
            {
                try
                {
                    _trackedProcesses.Add(process.Id, (process, DateTime.Now, process.TotalProcessorTime));
                }
                catch
                {
                    process.Dispose();
                }
            }
        }
    }

    private bool IsProcessRunning(Process process)
    {
        try
        {
            return !process.HasExited;
        }
        catch
        {
            return false;
        }
    }

    private bool IsImportantProcess(string processName)
    {
        return _importantProcessNames.Contains(processName.ToLower()) ||
               Process.GetCurrentProcess().ProcessName == processName;
    }

    public void Dispose()
    {
        foreach (var pair in _trackedProcesses)
        {
            try { pair.Value.Process.Dispose(); } catch { }
        }

        _trackedProcesses.Clear();
    }
}