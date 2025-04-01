using ProcessMonitor.Domain;

namespace ProcessMonitor.Application;
public interface IProcessService
{
    Task<List<ProcessInfoResponseDto>> GetAllProcessesAsync();
    Task<List<ProcessInfoResponseDto>> GetImportantProcessesAsync();
    Task<ProcessInfoResponseDto> GetProcessByIdAsync(int id);
    Task StartMonitoringAsync();
    Task StopMonitoringAsync();
    Task<bool> IsMonitoringAsync();
}