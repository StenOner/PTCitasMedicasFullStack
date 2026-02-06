using CitasMedicas.Application.DTOs;
using CitasMedicas.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Crear una nueva cita médica
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _appointmentService.CreateAppointmentAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors, message = result.Errors.FirstOrDefault() });
        }

        return CreatedAtAction(null, result.Data);
    }

    /// <summary>
    /// Obtener citas de un paciente
    /// </summary>
    [HttpGet("patient/{patientId}")]
    [ProducesResponseType(typeof(List<AppointmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientAppointments(int patientId)
    {
        var query = new PatientAppointmentsQuery
        {
            PatientId = patientId
        };

        var result = await _appointmentService.GetPatientAppointmentsAsync(query);

        if (!result.IsSuccess)
        {
            return NotFound(new { errors = result.Errors, message = result.Errors.FirstOrDefault() });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Cancelar una cita médica
    /// </summary>
    [HttpPatch("{appointmentId}/cancel")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelAppointment(
        int appointmentId,
        [FromBody] CancelAppointmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        dto.AppointmentId = appointmentId;

        var result = await _appointmentService.CancelAppointmentAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors, message = result.Errors.FirstOrDefault() });
        }

        return Ok(result.Data);
    }
}