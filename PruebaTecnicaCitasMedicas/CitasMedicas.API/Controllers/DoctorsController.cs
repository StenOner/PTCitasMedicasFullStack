using CitasMedicas.Application.DTOs;
using CitasMedicas.Application.Services;
using CitasMedicas.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    /// <summary>
    /// Buscar médicos por especialidad y/o tipo de atención
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<DoctorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchDoctors(
        [FromQuery] int? specialtyId,
        [FromQuery] CareType? careType)
    {
        var searchDto = new DoctorSearchDto
        {
            SpecialtyId = specialtyId,
            CareType = careType
        };

        var result = await _doctorService.SearchDoctorsAsync(searchDto);

        return Ok(result.Data);
    }

    /// <summary>
    /// Obtener médico por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var result = await _doctorService.GetDoctorByIdAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(new { errors = result.Errors, message = result.Errors.FirstOrDefault() });
        }

        return Ok(result.Data);
    }
}