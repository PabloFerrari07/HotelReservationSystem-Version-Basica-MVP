using HotelAplication.Dtos;

namespace HotelAplication.Services
{
    public interface IReservaService
    {
        Task<List<ReservaDto>> ObtenerReservasPorUsuario(int idUsuario);
        Task<ReservaDto> CrearReserva(int idUsuario, CrearReservaDto dto);
        Task CancelarReserva(int idReserva, int idUsuario);
        Task<HistorialReservasDto> ObtenerHistorialReservas(int idUsuario);

        Task<HistorialReservasDto> ObtenerHistorialCompleto();

        Task<DashBoardUsuarioDto> ObtenerDashboard(int idUsuario);

        Task<DashboardAdminDto> ObtenerDashboardAdmin();

        Task<List<ReservaDto>> ObtenerReservasConFiltros(FiltroReservaDto filtro);

        Task<List<OcupacionPorFechaDto>> ObtenerOcupacionPorFecha(FiltroOcupacionDto filtro);



    }
}
