using CitasMedicas.Application.DTOs;
using CitasMedicas.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly ISpecialtyService _specialityService;

    public SpecialtiesController(ISpecialtyService specialityService)
    {
        _specialityService = specialityService;
    }

    /// <summary>
    /// Obtener todas las especialidades
    /// </summary>
    [HttpGet()]
    [ProducesResponseType(typeof(List<SpecialtyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _specialityService.GetAllAsync();
        return Ok(result);
    }
}
