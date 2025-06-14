using HotelAplication.Dtos;

namespace HotelAplication.Services
{
    public interface IReservaService
    {
        Task<List<ReservaDto>> ObtenerReservasPorUsuario(int idUsuario);
        Task<ReservaDto> CrearReserva(int idUsuario, CrearReservaDto dto);
        Task CancelarReserva(int idReserva, int idUsuario);
    }
}
