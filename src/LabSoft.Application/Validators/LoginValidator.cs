using FluentValidation;
using LabSoft.Application.DTOs;

namespace LabSoft.Application.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.Password).NotEmpty(); // No validamos complejidad aquí, solo que no esté vacío
    }
}