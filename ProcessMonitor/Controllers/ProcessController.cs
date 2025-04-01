using Microsoft.AspNetCore.Mvc;
using ProcessMonitor.Application;
using ProcessMonitor.Domain;

namespace ProcessMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcessController : ControllerBase
{
    private readonly IProcessService _processService;

    public ProcessController(IProcessService processService)
    {
        _processService = processService;
    }

    /// <summary>
    /// Retrieves all currently running processes.
    /// </summary>
    /// <returns>A list of all running processes.</returns>
    [HttpGet]
    public async Task<ActionResult<List<ProcessInfo>>> GetAllProcesses()
    {
        return await _processService.GetAllProcessesAsync();
    }

    /// <summary>
    /// Retrieves a list of important processes (e.g., system-critical processes).
    /// </summary>
    /// <returns>A filtered list of important processes.</returns>
    [HttpGet("important")]
    public async Task<ActionResult<List<ProcessInfo>>> GetImportantProcesses()
    {
        return await _processService.GetImportantProcessesAsync();
    }

    /// <summary>
    /// Retrieves details of a specific process by its ID.
    /// </summary>
    /// <param name="id">The process ID.</param>
    /// <returns>The process details if found; otherwise, NotFound.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProcessInfo>> GetProcessById(int id)
    {
        var process = await _processService.GetProcessByIdAsync(id);
        if (process == null)
            return NotFound();

        return process;
    }

    /// <summary>
    /// Starts monitoring processes for high CPU usage.
    /// </summary>
    /// <returns>A message indicating that monitoring has started.</returns>
    [HttpPost("start-monitoring")]
    public IActionResult StartMonitoring()
    {
        _processService.StartMonitoringAsync();
        return Ok(new { message = "Process monitoring started" });
    }

    /// <summary>
    /// Stops monitoring processes.
    /// </summary>
    /// <returns>A message indicating that monitoring has stopped.</returns>
    [HttpPost("stop-monitoring")]
    public async Task<IActionResult> StopMonitoring()
    {
        await _processService.StopMonitoringAsync();
        return Ok(new { message = "Process monitoring stopped" });
    }

    /// <summary>
    /// Retrieves the current monitoring status.
    /// </summary>
    /// <returns>True if monitoring is active, otherwise false.</returns>
    [HttpGet("monitoring-status")]
    public async Task<IActionResult> GetMonitoringStatus()
    {
        bool isMonitoring = await _processService.IsMonitoringAsync();
        return Ok(new { isMonitoring });
    }
}
