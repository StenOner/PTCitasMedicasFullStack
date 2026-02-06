using CitasMedicas.Application.Common;
using CitasMedicas.Application.DTOs;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Enums;
using CitasMedicas.Domain.Interfaces;

namespace CitasMedicas.Application.Services;

public interface IAppointmentService
{
    Task<Result<AppointmentDto>> CreateAppointmentAsync(CreateAppointmentDto dto);
    Task<Result<List<AppointmentDto>>> GetPatientAppointmentsAsync(PatientAppointmentsQuery query);
    Task<Result<AppointmentDto>> CancelAppointmentAsync(CancelAppointmentDto dto);
}

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AppointmentDto>> CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        try
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(dto.PatientId);
            if (patient == null || !patient.IsActive)
            {
                return Result<AppointmentDto>.Failure("Paciente no encontrado");
            }

            var schedule = await _unitOfWork.DoctorSchedules.GetByIdAsync(dto.ScheduleId);
            if (schedule == null || !schedule.IsActive)
            {
                return Result<AppointmentDto>.Failure("Horario no encontrado");
            }

            if (!schedule.IsAvailable)
            {
                return Result<AppointmentDto>.Failure("El horario seleccionado ya no está disponible");
            }

            var appointmentDateTime = schedule.ScheduleDate.Add(schedule.StartTime);
            if (appointmentDateTime < DateTime.Now)
            {
                return Result<AppointmentDto>.Failure("No se pueden reservar citas en fechas u horarios pasados");
            }

            var existingAppointment = await _unitOfWork.Appointments.FindOneAsync(x =>
                x.ScheduleId == dto.ScheduleId &&
                x.Status != AppointmentStatus.Cancelada &&
                x.IsActive
            );

            if (existingAppointment != null)
            {
                return Result<AppointmentDto>.Failure("Este horario ya ha sido reservado por otro paciente");
            }

            var patientConflictingAppointment = await _unitOfWork.Appointments.FindOneAsync(x =>
                x.PatientId == dto.PatientId &&
                x.AppointmentDate == schedule.ScheduleDate &&
                x.StartTime == schedule.StartTime &&
                x.Status != AppointmentStatus.Cancelada &&
                x.IsActive
            );

            if (patientConflictingAppointment != null)
            {
                return Result<AppointmentDto>.Failure("Ya tiene una cita programada en este horario");
            }

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(schedule.DoctorId);
            if (doctor == null || !doctor.IsActive)
            {
                return Result<AppointmentDto>.Failure("Médico no encontrado");
            }

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = schedule.DoctorId,
                ScheduleId = dto.ScheduleId,
                AppointmentDate = schedule.ScheduleDate,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Status = AppointmentStatus.Programada,
                Notes = dto.Notes
            };

            await _unitOfWork.Appointments.AddAsync(appointment);

            schedule.IsAvailable = false;
            _unitOfWork.DoctorSchedules.Update(schedule);

            await _unitOfWork.SaveChangesAsync();

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientName = $"{patient.FirstName} {patient.LastName}",
                DoctorId = appointment.DoctorId,
                DoctorName = $"Dr. {doctor.FirstName} {doctor.LastName}",
                SpecialtyName = doctor.Specialty.Name ?? "No Definida",
                AppointmentDate = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                Notes = appointment.Notes
            };

            return Result<AppointmentDto>.Success(appointmentDto);
        }
        catch (Exception ex)
        {
            return Result<AppointmentDto>.Failure($"Error al crear la cita: {ex.Message}");
        }
    }

    public async Task<Result<List<AppointmentDto>>> GetPatientAppointmentsAsync(PatientAppointmentsQuery query)
    {
        var patient = await _unitOfWork.Patients.GetByIdAsync(query.PatientId);
        if (patient == null || !patient.IsActive)
        {
            return Result<List<AppointmentDto>>.Failure("Paciente no encontrado");
        }

        var appointments = await _unitOfWork.Appointments.FindAsync(x =>
            x.PatientId == query.PatientId &&
            x.IsActive
        );

        var appointmentsList = appointments
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToList();

        var appointmentDtos = appointmentsList.Select(a => new AppointmentDto
        {
            Id = a.Id,
            PatientId = a.PatientId,
            PatientName = $"{patient.FirstName} {patient.LastName}",
            DoctorId = a.DoctorId,
            DoctorName = a.Doctor is not null ? $"Dr. {a.Doctor.FirstName} {a.Doctor.LastName}" : "",
            SpecialtyName = a.Doctor?.Specialty?.Name ?? "No Definida",
            AppointmentDate = a.AppointmentDate,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            Notes = a.Notes,
            CancellationReason = a.CancellationReason
        }).ToList();

        return Result<List<AppointmentDto>>.Success(appointmentDtos);
    }

    public async Task<Result<AppointmentDto>> CancelAppointmentAsync(CancelAppointmentDto dto)
    {
        try
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(dto.AppointmentId);
            if (appointment == null || !appointment.IsActive)
            {
                return Result<AppointmentDto>.Failure("Cita no encontrada");
            }

            if (appointment.Status == AppointmentStatus.Cancelada)
            {
                return Result<AppointmentDto>.Failure("Esta cita ya ha sido cancelada");
            }

            var appointmentDateTime = appointment.AppointmentDate.Add(appointment.StartTime);
            if (appointmentDateTime < DateTime.Now)
            {
                return Result<AppointmentDto>.Failure("Solo se pueden cancelar citas futuras");
            }

            appointment.Status = AppointmentStatus.Cancelada;
            appointment.CancellationReason = dto.CancellationReason;
            appointment.CancelledAt = DateTime.UtcNow;
            _unitOfWork.Appointments.Update(appointment);

            var schedule = await _unitOfWork.DoctorSchedules.GetByIdAsync(appointment.ScheduleId);
            if (schedule != null)
            {
                schedule.IsAvailable = true;
                _unitOfWork.DoctorSchedules.Update(schedule);
            }

            await _unitOfWork.SaveChangesAsync();

            var appointmentDto = new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient is not null ? $"{appointment.Patient.FirstName} {appointment.Patient.LastName}" : "",
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor is not null ? $"Dr. {appointment.Doctor.FirstName} {appointment.Doctor.LastName}" : "",
                SpecialtyName = appointment.Doctor?.Specialty?.Name ?? "No Definida",
                AppointmentDate = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                Notes = appointment.Notes,
                CancellationReason = appointment.CancellationReason
            };

            return Result<AppointmentDto>.Success(appointmentDto);
        }
        catch (Exception ex)
        {
            return Result<AppointmentDto>.Failure($"Error al cancelar la cita: {ex.Message}");
        }
    }
}