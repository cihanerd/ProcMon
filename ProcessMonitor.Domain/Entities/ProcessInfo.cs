
namespace ProcessMonitor.Domain;
public class ProcessInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double CpuUsage { get; set; }
    public long MemoryUsage { get; set; }
    public string Status { get; set; }
    public DateTime StartTime { get; set; }
    public TimeSpan TotalProcessorTime { get; set; }
    public int ThreadCount { get; set; }
    public bool IsImportant { get; set; }
}

