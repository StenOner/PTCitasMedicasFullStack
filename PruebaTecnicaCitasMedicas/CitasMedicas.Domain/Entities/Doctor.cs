using CitasMedicas.Domain.Enums;

namespace CitasMedicas.Domain.Entities;

public class Doctor : BaseEntity
{
    public string LicenseNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int SpecialtyId { get; set; }
    public CareType CareType { get; set; }

    // Relaciones
    public virtual Specialty Specialty { get; set; } = null!;
    public virtual ICollection<DoctorSchedule> Schedules { get; set; } = [];
    public virtual ICollection<Appointment> Appointments { get; set; } = [];
}
