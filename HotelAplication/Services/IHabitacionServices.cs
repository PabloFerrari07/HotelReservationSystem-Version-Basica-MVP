using HotelAplication.Dtos;

namespace HotelAplication.Services
{
    public interface IHabitacionServices
    {
        Task<IEnumerable<HabitacionDto>> ObtenerHabitaciones();
        Task<HabitacionDto> AgregarHabitacion(HabitacionDto dto);
        Task<HabitacionDto> ObtenerHabitacionPorNumero(int numero);
        Task<HabitacionDto> EditarHabitacion(int id, HabitacionDto dto);
        Task<List<HabitacionDto>> ObtenerHabitacionesDisponibles(DateTime fechaInicio, DateTime fechaFin);

    }
}
