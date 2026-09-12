using FluentValidation;
using LabSoft.Application.DTOs;
using LabSoft.Domain.Entities;

namespace LabSoft.Application.Validators;

public class PacienteValidator : AbstractValidator<PacienteDto>
{
    public PacienteValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.");

        RuleFor(p => p.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.");

        RuleFor(p => p.FechaNacimiento)
            .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de nacimiento no puede ser en el futuro.");

        RuleFor(p => p.Sexo)
            .IsInEnum().WithMessage("Debe seleccionar un sexo válido.");

        // Regla de negocio: Si es recién nacido, la cédula debe venir vacía/nula. Si no, es obligatoria.
        When(p => p.EsRecienNacido, () =>
        {
            RuleFor(p => p.Cedula)
                .Empty().WithMessage("Un recién nacido o menor sin cédula no debe registrar número de cédula.");
        })
        .Otherwise(() =>
        {
            RuleFor(p => p.Cedula)
                .NotEmpty().WithMessage("La cédula es obligatoria para pacientes regulares.");
        });

        // Si selecciona un parentesco que requiere titular (ej. Hijo, Cónyuge, etc.), exigimos que venga el TitularId
        When(p => p.Parentesco.HasValue && p.Parentesco != ParentescoEnum.Titular, () =>
        {
            RuleFor(p => p.TitularId)
                .NotEmpty().NotNull().WithMessage("Debe asociar un paciente titular válido para este beneficiario.");
        });
    }
}