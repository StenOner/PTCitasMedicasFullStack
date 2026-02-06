using CitasMedicas.Application.Common;
using CitasMedicas.Application.DTOs;
using CitasMedicas.Domain.Interfaces;

namespace CitasMedicas.Application.Services;

public interface ISpecialtyService
{
    Task<Result<List<SpecialtyDto>>> GetAllAsync();
}

public class SpecialtyService : ISpecialtyService
{
    private readonly IUnitOfWork _unitOfWork;

    public SpecialtyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<SpecialtyDto>>> GetAllAsync()
    {
        var specialties = await _unitOfWork.Specialties.GetAllAsync();
        var specialtyDtos = specialties.Select(x => new SpecialtyDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }).ToList();

        return Result<List<SpecialtyDto>>.Success(specialtyDtos);
    }
}