using FluentValidation;
using HotelAplication.Dtos;

namespace HotelAplication.Validators
{
    public class ReservaValidator : AbstractValidator<CrearReservaDto>
    {
        public ReservaValidator()
        {
            RuleFor(x => x.IdHabitacion)
                .GreaterThan(0).WithMessage("La habitación debe ser válida.");

            RuleFor(x => x.FechaEntrada)
                .GreaterThan(DateTime.Today).WithMessage("La fecha de entrada debe ser posterior a hoy.");

            RuleFor(x => x.FechaSalida)
                .GreaterThan(x => x.FechaEntrada).WithMessage("La fecha de salida debe ser posterior a la de entrada.");
        }
    }
}
