using CitasMedicas.Application.DTOs;
using FluentValidation;

namespace CitasMedicas.Application.Validators;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("El ID del paciente es requerido");

        RuleFor(x => x.ScheduleId)
            .GreaterThan(0).WithMessage("El ID del horario es requerido");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Las notas no pueden exceder 1000 caracteres");
    }
}
