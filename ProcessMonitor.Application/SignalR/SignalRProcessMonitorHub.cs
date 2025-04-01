using Microsoft.AspNetCore.SignalR;
using ProcessMonitor.Domain;
using ProcessMonitor.Infrastructure;
using ProcessMonitor.Presentation;

namespace ProcessMonitor.Application;

public class SignalRProcessMonitorHub : IProcessMonitorHub, IDisposable
{
    private readonly IProcessRepository _processRepository;
    private readonly IHubContext<ProcessMonitorHub> _hubContext;
    private CancellationTokenSource? _cts;
    private Task? _monitoringTask;
    private bool _isMonitoring;
    private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(200);

    public SignalRProcessMonitorHub(IProcessRepository processRepository, IHubContext<ProcessMonitorHub> hubContext)
    {
        _processRepository = processRepository;
        _hubContext = hubContext;
    }

    public async Task BroadcastProcessesAsync(List<ProcessInfoResponseDto> processes)
    {
        await _hubContext.Clients.All.SendAsync(SignalRConsts.ReceiveProcesses, processes);
    }

    public async Task BroadcastErrorAsync(string errorMessage)
    {
        await _hubContext.Clients.All.SendAsync(SignalRConsts.Error, errorMessage);
    }

    public Task<bool> IsMonitoring()
    {
        return Task.FromResult(_isMonitoring);
    }

    public async Task StartMonitoring()
    {
        if (_isMonitoring)
            return;

        _cts = new CancellationTokenSource();
        _isMonitoring = true;

        _monitoringTask = Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                try
                {
                    var importantProcesses = await _processRepository.GetImportantProcessesAsync();
                    await BroadcastProcessesAsync(importantProcesses.ToDtoList());
                    await Task.Delay(_updateInterval, _cts.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    await BroadcastErrorAsync($"Monitoring error: {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token);
                }
            }

            _isMonitoring = false;
        }, _cts.Token);
        await Task.CompletedTask;
    }

    public async Task StopMonitoring()
    {
        if (!_isMonitoring)
            return;

        _cts?.Cancel();

        try
        {
            if (_monitoringTask != null)
                await _monitoringTask;
        }
        catch (OperationCanceledException)
        {
            // this is expected. Do nothing!
        }
        finally
        {
            _monitoringTask = null;
        }

        _isMonitoring = false;
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}