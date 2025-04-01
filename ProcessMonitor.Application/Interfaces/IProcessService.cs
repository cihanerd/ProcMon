using ProcessMonitor.Domain;

namespace ProcessMonitor.Application;
public interface IProcessService
{
    Task<List<ProcessInfo>> GetAllProcessesAsync();
    Task<List<ProcessInfo>> GetImportantProcessesAsync();
    Task<ProcessInfo> GetProcessByIdAsync(int id);
    Task StartMonitoringAsync();
    Task StopMonitoringAsync();
    Task<bool> IsMonitoringAsync();
}