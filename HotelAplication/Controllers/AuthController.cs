using HotelAplication.Dtos;
using HotelAplication.Models;
using HotelAplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HotelContext _context;
        private readonly JwtService _jwtService;
        private readonly IAuthService _authService;
        public AuthController(HotelContext context, JwtService jwtService, IAuthService authService)
        {
            _context = context;
            _jwtService = jwtService;
            _authService = authService;
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<ActionResult<UsuarioDto>> RegistrarUsuario(RegistroDto dto)
        {
            var usuarioDto = await _authService.RegistrarUsuario(dto);
            return Ok(usuarioDto);

        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> LoginUsuario(LoginDto dto)
        {
            var usuario = await _authService.Login(dto);
            if (usuario == null)
                return Unauthorized("Email o contraseña incorrectos.");

            return Ok(usuario);

    
        }
    }
}
