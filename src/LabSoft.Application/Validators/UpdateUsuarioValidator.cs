using FluentValidation;
using LabSoft.Application.DTOs;

namespace LabSoft.Application.Validators;

public class UpdateUsuarioValidator : AbstractValidator<UpdateUsuarioDto>
{
    public UpdateUsuarioValidator()
    {
        // El email es opcional, pero si lo escribe, debe tener formato válido
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El formato del correo no es válido.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        // La contraseña es opcional, pero si la cambia, debe cumplir con la seguridad
        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage("La nueva contraseña debe tener al menos 6 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Password));

        // Teléfono y domicilio opcionales con reglas básicas 
        RuleFor(x => x.Telefono)
            .MaximumLength(20).WithMessage("El teléfono es demasiado largo.")
            .When(x => !string.IsNullOrEmpty(x.Telefono));
    }
}