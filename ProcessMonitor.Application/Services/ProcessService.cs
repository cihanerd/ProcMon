using ProcessMonitor.Domain;
using ProcessMonitor.Infrastructure;
using ProcessMonitor.Presentation;

namespace ProcessMonitor.Application;
public class ProcessService : IProcessService
{
    private readonly IProcessRepository _processRepository;
    private readonly IProcessMonitorHub _processMonitorHub;

    public ProcessService(IProcessRepository processRepository, IProcessMonitorHub processMonitorHub)
    {
        _processRepository = processRepository;
        _processMonitorHub = processMonitorHub;
    }

    public async Task<List<ProcessInfo>> GetAllProcessesAsync()
    {
        return await _processRepository.GetAllProcessesAsync();
    }

    public async Task<List<ProcessInfo>> GetImportantProcessesAsync()
    {
        return await _processRepository.GetImportantProcessesAsync();
    }

    public async Task<ProcessInfo> GetProcessByIdAsync(int id)
    {
        return await _processRepository.GetProcessByIdAsync(id);
    }

    public async Task<bool> IsMonitoringAsync()
    {
        return await _processMonitorHub.IsMonitoring();
    }

    public async Task StartMonitoringAsync()
    {
        await _processMonitorHub.StartMonitoring();
    }

    public async Task StopMonitoringAsync()
    {
       await _processMonitorHub.StopMonitoring();
    }
}