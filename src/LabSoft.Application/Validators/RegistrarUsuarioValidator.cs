using FluentValidation;
using LabSoft.Application.DTOs;
using LabSoft.Domain.Constants;

namespace LabSoft.Application.Validators;

public class RegistrarUsuarioValidator : AbstractValidator<RegistrarUsuarioDto>
{
    public RegistrarUsuarioValidator()
    {
        // Validación de Nombre de Usuario
        RuleFor(u => u.NombreUsuario)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
            .Length(3, 50).WithMessage("El nombre de usuario debe tener entre 3 y 50 caracteres.")
            .Matches("^[a-zA-Z0-9_]*$").WithMessage("El nombre de usuario solo puede contener letras, números y guiones bajos.");

        // Validación de Correo Electrónico
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido (ej. usuario@dominio.com).")
            .MaximumLength(100).WithMessage("El correo electrónico no puede superar los 100 caracteres.");

        // Validación de Contraseña Segura
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .MaximumLength(100).WithMessage("La contraseña no puede superar los 100 caracteres.")
            .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
            .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
            .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número.")
            .Matches("[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial (ej. @, #, $, !).");

        // Validación de Rol
        RuleFor(u => u.Rol)
            .NotEmpty().WithMessage("El rol es obligatorio.")
            .Must(rol => RolesConstantes.EsValido(rol))
            .WithMessage($"El rol especificado no es válido. Roles permitidos: {string.Join(", ", RolesConstantes.Todos)}");
    }
}