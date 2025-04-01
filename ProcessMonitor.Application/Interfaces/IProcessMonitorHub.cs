using ProcessMonitor.Domain;

namespace ProcessMonitor.Application;

public interface IProcessMonitorHub
{
    Task BroadcastProcessesAsync(List<ProcessInfoResponseDto> processes);
    Task BroadcastErrorAsync(string errorMessage);

    Task<bool> IsMonitoring();

    Task StartMonitoring();
    Task StopMonitoring();
}
