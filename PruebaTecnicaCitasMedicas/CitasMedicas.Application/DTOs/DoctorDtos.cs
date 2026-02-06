using CitasMedicas.Domain.Enums;

namespace CitasMedicas.Application.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"Dr. {FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int SpecialtyId { get; set; }
    public string SpecialtyName { get; set; } = string.Empty;
    public CareType CareType { get; set; }
    public string CareTypeName => CareType.ToString();
}

public class DoctorSearchDto
{
    public int? SpecialtyId { get; set; }
    public CareType? CareType { get; set; }
}