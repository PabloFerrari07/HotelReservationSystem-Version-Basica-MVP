using FluentValidation;
using HotelAplication.Dtos;

namespace HotelAplication.Validators
{
    public class RegisterValidator : AbstractValidator<RegistroDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor (x => x.Nombre).NotEmpty();
            RuleFor(x => x.Rol)
                .Must(rol => string.IsNullOrEmpty(rol) || rol == "admin" || rol == "cliente")
                .WithMessage("El rol debe ser 'admin' o 'cliente'.");

        }
    }
}
