namespace CitasMedicas.Domain.Entities;

public class Patient : BaseEntity
{
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;

    public virtual ICollection<Appointment> Appointments { get; set; } = [];
}
