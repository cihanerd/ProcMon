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

    public async Task<List<ProcessInfoResponseDto>> GetAllProcessesAsync()
    {
        var res = await _processRepository.GetAllProcessesAsync();
        return res.ToDtoList();
    }

    public async Task<List<ProcessInfoResponseDto>> GetImportantProcessesAsync()
    {
        var res = await _processRepository.GetImportantProcessesAsync();
        return res.ToDtoList();
    }

    public async Task<ProcessInfoResponseDto> GetProcessByIdAsync(int id)
    {
        var res = await _processRepository.GetProcessByIdAsync(id);
        return res.ToDto();
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