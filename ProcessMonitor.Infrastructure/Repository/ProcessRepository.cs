using ProcessMonitor.Domain;

namespace ProcessMonitor.Infrastructure;

public class ProcessRepository : IProcessRepository, IDisposable
{
    private readonly ProcessTracker _processTracker;

    public ProcessRepository()
    {
        _processTracker = new ProcessTracker();
    }

    public async Task<List<ProcessInfo>> GetAllProcessesAsync()
    {
        return await Task.FromResult(_processTracker.GetProcessInfo());
    }

    public async Task<List<ProcessInfo>> GetImportantProcessesAsync()
    {
        var processes = await GetAllProcessesAsync();
        return processes.Where(p => p.IsImportant).ToList();
    }

    public async Task<ProcessInfo> GetProcessByIdAsync(int id)
    {
        var processes = await GetAllProcessesAsync();
        return processes.FirstOrDefault(p => p.Id == id);
    }

    public void Dispose()
    {
        _processTracker?.Dispose();
    }
}
