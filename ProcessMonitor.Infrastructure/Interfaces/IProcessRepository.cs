using ProcessMonitor.Domain;

namespace ProcessMonitor.Infrastructure;

public interface IProcessRepository
{
    Task<List<ProcessInfo>> GetAllProcessesAsync();
    Task<List<ProcessInfo>> GetImportantProcessesAsync();
    Task<ProcessInfo> GetProcessByIdAsync(int id);
}
