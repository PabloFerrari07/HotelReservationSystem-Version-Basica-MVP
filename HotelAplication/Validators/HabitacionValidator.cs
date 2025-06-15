using FluentValidation;
using HotelAplication.Dtos;

namespace HotelAplication.Validators
{
    public class HabitacionValidator : AbstractValidator<HabitacionDto>
    {
        public HabitacionValidator()
        {
            RuleFor(x => x.Numero).NotNull().WithMessage("El número de habitación es obligatorio.");
            RuleFor(x => x.Tipo).NotEmpty().WithMessage("El tipo es obligatorio.");
            RuleFor(x => x.Disponible).NotNull().WithMessage("La disponibilidad es obligatoria.");
            RuleFor(x => x.PrecioPorNoche).NotNull().GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
        }
    }
}
