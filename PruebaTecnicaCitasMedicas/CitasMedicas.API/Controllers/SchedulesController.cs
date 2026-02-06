using CitasMedicas.Application.DTOs;
using CitasMedicas.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    /// <summary>
    /// Obtener horarios disponibles de un médico
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(List<ScheduleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAvailableSchedules(
        [FromQuery] int doctorId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        if (doctorId <= 0)
        {
            return BadRequest(new { message = "El ID del médico es requerido" });
        }

        var query = new AvailableScheduleQuery
        {
            DoctorId = doctorId,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await _scheduleService.GetAvailableSchedulesAsync(query);

        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors, message = result.Errors.FirstOrDefault() });
        }

        return Ok(result.Data);
    }
}
