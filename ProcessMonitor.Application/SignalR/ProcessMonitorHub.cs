using Microsoft.AspNetCore.SignalR;

namespace ProcessMonitor.Application;

public class ProcessMonitorHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
