namespace CitasMedicas.Application.DTOs;

public class ScheduleDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime ScheduleDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; }
    public string TimeRange => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
}

public class AvailableScheduleQuery
{
    public int DoctorId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}