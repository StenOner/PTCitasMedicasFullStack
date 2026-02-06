using CitasMedicas.Application.Common;
using CitasMedicas.Application.DTOs;
using CitasMedicas.Domain.Interfaces;

namespace CitasMedicas.Application.Services;

public interface IScheduleService
{
    Task<Result<List<ScheduleDto>>> GetAvailableSchedulesAsync(AvailableScheduleQuery query);
}

public class ScheduleService : IScheduleService
{
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ScheduleDto>>> GetAvailableSchedulesAsync(AvailableScheduleQuery query)
    {
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(query.DoctorId);
        if (doctor == null || !doctor.IsActive)
        {
            return Result<List<ScheduleDto>>.Failure("Médico no encontrado");
        }

        var startDate = query.StartDate ?? DateTime.Today;
        var endDate = query.EndDate ?? DateTime.Today.AddDays(30);

        if (startDate < DateTime.Today)
        {
            return Result<List<ScheduleDto>>.Failure("Fecha de inicio no puede ser antes que hoy");
        }

        var schedules = await _unitOfWork.DoctorSchedules.FindAsync(x =>
            x.DoctorId == query.DoctorId &&
            x.IsAvailable &&
            x.IsActive &&
            x.ScheduleDate >= startDate &&
            x.ScheduleDate <= endDate
        );

        var schedulesList = schedules
            .OrderBy(x => x.ScheduleDate)
            .ThenBy(x => x.StartTime)
            .ToList();

        var scheduleDtos = schedulesList.Select(x => new ScheduleDto
        {
            Id = x.Id,
            DoctorId = x.DoctorId,
            DoctorName = $"Dr. {doctor.FirstName} {doctor.LastName}",
            ScheduleDate = x.ScheduleDate,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            IsAvailable = x.IsAvailable
        }).ToList();

        return Result<List<ScheduleDto>>.Success(scheduleDtos);
    }
}