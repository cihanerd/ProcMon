namespace ProcessMonitor.Domain;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ProcessId { get; set; }
    public string ProcessName { get; set; }

    public Notification(string message, int processId, string processName)
    {
        Id = Guid.NewGuid().GetHashCode();
        CreatedAt = DateTime.Now;
        Message = message;
        ProcessId = processId;
        ProcessName = processName;
    }
}
