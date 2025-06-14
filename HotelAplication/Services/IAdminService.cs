using HotelAplication.Dtos;

namespace HotelAplication.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<UsuarioDto>> ObtenerUsuarios();
        Task<UsuarioDto> ObtenerUsuarioPorId(int id);
        Task<UsuarioDto> EditarUsuario(int id, UsuarioDto dto);
        Task<UsuarioDto> EliminarUsuario(int id);
    }
}
