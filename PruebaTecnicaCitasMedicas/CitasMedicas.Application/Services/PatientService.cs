using CitasMedicas.Application.Common;
using CitasMedicas.Application.DTOs;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces;

namespace CitasMedicas.Application.Services;

public interface IPatientService
{
    Task<Result<PatientDto>> CreatePatientAsync(CreatePatientDto dto);
    Task<Result<PatientDto>> GetPatientByIdAsync(int id);
    Task<Result<PatientDto>> GetPatientByDocumentAsync(string documentNumber);
}

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PatientDto>> CreatePatientAsync(CreatePatientDto dto)
    {
        var existingPatient = await _unitOfWork.Patients
            .FindOneAsync(x => x.DocumentNumber == dto.DocumentNumber && x.IsActive);

        if (existingPatient != null)
        {
            return Result<PatientDto>.Failure("Ya existe un paciente registrado con este número de documento");
        }

        var existingEmail = await _unitOfWork.Patients
            .FindOneAsync(x => x.Email == dto.Email && x.IsActive);

        if (existingEmail != null)
        {
            return Result<PatientDto>.Failure("Ya existe un paciente registrado con este email");
        }

        var patient = new Patient
        {
            DocumentNumber = dto.DocumentNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            DateOfBirth = dto.DateOfBirth,
            Address = dto.Address
        };

        await _unitOfWork.Patients.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        var patientDto = new PatientDto
        {
            Id = patient.Id,
            DocumentNumber = patient.DocumentNumber,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            Phone = patient.Phone,
            DateOfBirth = patient.DateOfBirth,
            Address = patient.Address
        };

        return Result<PatientDto>.Success(patientDto);
    }

    public async Task<Result<PatientDto>> GetPatientByIdAsync(int id)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(id);

        if (patient == null || !patient.IsActive)
        {
            return Result<PatientDto>.Failure("Paciente no encontrado");
        }

        var patientDto = new PatientDto
        {
            Id = patient.Id,
            DocumentNumber = patient.DocumentNumber,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            Phone = patient.Phone,
            DateOfBirth = patient.DateOfBirth,
            Address = patient.Address
        };

        return Result<PatientDto>.Success(patientDto);
    }

    public async Task<Result<PatientDto>> GetPatientByDocumentAsync(string documentNumber)
    {
        var patient = await _unitOfWork.Patients
            .FindOneAsync(x => x.DocumentNumber == documentNumber && x.IsActive);

        if (patient == null)
        {
            return Result<PatientDto>.Failure("Paciente no encontrado");
        }

        var patientDto = new PatientDto
        {
            Id = patient.Id,
            DocumentNumber = patient.DocumentNumber,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            Phone = patient.Phone,
            DateOfBirth = patient.DateOfBirth,
            Address = patient.Address
        };

        return Result<PatientDto>.Success(patientDto);
    }
}