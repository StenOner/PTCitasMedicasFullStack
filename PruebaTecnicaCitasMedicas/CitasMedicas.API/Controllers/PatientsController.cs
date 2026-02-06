using CitasMedicas.Application.DTOs;
using CitasMedicas.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Registrar un nuevo paciente
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterPatient([FromBody] CreatePatientDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _patientService.CreatePatientAsync(dto);

        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors, message = result.Errors.FirstOrDefault() });
        }

        return CreatedAtAction(null, result.Data);
    }

    /// <summary>
    /// Obtener paciente por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientById(int id)
    {
        var result = await _patientService.GetPatientByIdAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(new { errors = result.Errors, message = result.Errors.FirstOrDefault() });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Obtener paciente por número de documento
    /// </summary>
    [HttpGet("by-document/{documentNumber}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientByDocument(string documentNumber)
    {
        var result = await _patientService.GetPatientByDocumentAsync(documentNumber);

        if (!result.IsSuccess)
        {
            return NotFound(new { errors = result.Errors, message = result.Errors.FirstOrDefault() });
        }

        return Ok(result.Data);
    }
}