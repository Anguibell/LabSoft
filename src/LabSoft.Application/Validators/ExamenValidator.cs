using FluentValidation;
using LabSoft.Application.DTOs;

namespace LabSoft.Application.Validators;

public class ExamenValidator : AbstractValidator<ExamenDto>
{
    public ExamenValidator()
    {
        RuleFor(x => x.Codigo)
            .GreaterThan(0)
            .WithMessage("El código del examen debe ser un número entero válido.");

        // OTRAS REGLAS EXISTENTES...
    }
}