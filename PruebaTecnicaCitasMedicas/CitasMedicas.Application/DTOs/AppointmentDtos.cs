using CitasMedicas.Domain.Enums;

namespace CitasMedicas.Application.DTOs;

public class CreateAppointmentDto
{
    public int PatientId { get; set; }
    public int ScheduleId { get; set; }
    public string? Notes { get; set; }
}

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string SpecialtyName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string TimeRange => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
    public AppointmentStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }
}

public class CancelAppointmentDto
{
    public int AppointmentId { get; set; }
    public string CancellationReason { get; set; } = string.Empty;
}

public class PatientAppointmentsQuery
{
    public int PatientId { get; set; }
}