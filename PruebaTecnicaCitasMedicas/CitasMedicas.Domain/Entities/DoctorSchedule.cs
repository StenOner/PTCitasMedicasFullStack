namespace CitasMedicas.Domain.Entities;

public class DoctorSchedule : BaseEntity
{
    public int DoctorId { get; set; }
    public DateTime ScheduleDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Relaciones
    public virtual Doctor Doctor { get; set; } = null!;
}
