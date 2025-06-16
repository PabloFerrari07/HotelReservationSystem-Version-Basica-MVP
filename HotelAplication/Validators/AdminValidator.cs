using FluentValidation;
using HotelAplication.Dtos;

namespace HotelAplication.Validators
{
    public class AdminValidator : AbstractValidator<UsuarioDto>
    {
        public AdminValidator() {
            RuleFor(x => x.Email).NotEmpty().WithMessage("El campo Email es obligatorio");
            RuleFor(x => x.Name).NotEmpty().WithMessage("El campo Nombre es obligatorio");
            RuleFor(x => x.Rol).NotEmpty()
                .Must(rol => string.IsNullOrEmpty(rol) || rol == "admin" || rol == "cliente")
                .WithMessage("El rol debe ser 'admin' o 'cliente'.");
        }
    }
}
