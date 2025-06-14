using HotelAplication.Dtos;

namespace HotelAplication.Services
{
    public interface IAuthService
    {

            Task<UsuarioDto> RegistrarUsuario(RegistroDto dto);
            Task<UsuarioDto?> Login(LoginDto dto);
        

    }
}
