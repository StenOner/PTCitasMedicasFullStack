using CitasMedicas.Application.Common;
using CitasMedicas.Application.DTOs;
using CitasMedicas.Domain.Interfaces;

namespace CitasMedicas.Application.Services;

public interface IDoctorService
{
    Task<Result<List<DoctorDto>>> SearchDoctorsAsync(DoctorSearchDto searchDto);
    Task<Result<DoctorDto>> GetDoctorByIdAsync(int id);
}

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<DoctorDto>>> SearchDoctorsAsync(DoctorSearchDto searchDto)
    {
        var doctors = await _unitOfWork.Doctors.GetAllAsync();

        var query = doctors.AsQueryable();

        if (searchDto.SpecialtyId.HasValue)
        {
            query = query.Where(x => x.SpecialtyId == searchDto.SpecialtyId.Value);
        }

        if (searchDto.CareType.HasValue)
        {
            query = query.Where(x => x.CareType == searchDto.CareType.Value);
        }

        var doctorsList = query.ToList();

        var doctorDtos = doctorsList.Select(x => new DoctorDto
        {
            Id = x.Id,
            LicenseNumber = x.LicenseNumber,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Phone = x.Phone,
            SpecialtyId = x.SpecialtyId,
            SpecialtyName = x.Specialty.Name ?? "No Definida",
            CareType = x.CareType
        }).ToList();

        return Result<List<DoctorDto>>.Success(doctorDtos);
    }

    public async Task<Result<DoctorDto>> GetDoctorByIdAsync(int id)
    {
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);

        if (doctor == null || !doctor.IsActive)
        {
            return Result<DoctorDto>.Failure("Médico no encontrado");
        }

        var doctorDto = new DoctorDto
        {
            Id = doctor.Id,
            LicenseNumber = doctor.LicenseNumber,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Email = doctor.Email,
            Phone = doctor.Phone,
            SpecialtyId = doctor.SpecialtyId,
            SpecialtyName = doctor.Specialty.Name ?? "No Definida",
            CareType = doctor.CareType
        };

        return Result<DoctorDto>.Success(doctorDto);
    }
}