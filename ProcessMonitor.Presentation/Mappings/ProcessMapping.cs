using ProcessMonitor.Domain;

namespace ProcessMonitor.Presentation;
public static class ProcessMapping
{
    public static ProcessInfoResponseDto ToDto(this ProcessInfo process)
    {
        return new ProcessInfoResponseDto
        {
            Id = process.Id,
            Name = process.Name,
            CpuUsage = process.CpuUsage,
            MemoryUsage = process.MemoryUsage,
            Status = process.Status,
            StartTime = process.StartTime,
            TotalProcessorTime = process.TotalProcessorTime,
            ThreadCount = process.ThreadCount,
            IsImportant = process.IsImportant,
        };
    }

    public static List<ProcessInfoResponseDto> ToDtoList(this List<ProcessInfo> processes)
    {
        return processes.Select(p => p.ToDto()).ToList();
    }
}

