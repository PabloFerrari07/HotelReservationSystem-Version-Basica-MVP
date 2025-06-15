using FluentValidation;
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

        private readonly IValidator <RegistroDto> _registroValidator ;
        private readonly IValidator <LoginDto> _LoginValidator;
        private readonly IAuthService _authService;
        public AuthController(HotelContext context, JwtService jwtService,
                              IAuthService authService,
                              IValidator<RegistroDto> registroValidator,
                              IValidator<LoginDto> loginValidator)
        {
            _context = context;
            _jwtService = jwtService;
            _authService = authService;
            _registroValidator = registroValidator;
            _LoginValidator = loginValidator;
        }

        [HttpPost]
        [Route("Registro")]
        public async Task<ActionResult<UsuarioDto>> RegistrarUsuario(RegistroDto dto)
        {
            var validationResult = await _registroValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var usuarioDto = await _authService.RegistrarUsuario(dto);
            return Ok(usuarioDto);

        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> LoginUsuario(LoginDto dto)
        {
            var validationResult = await _LoginValidator.ValidateAsync(dto);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var usuario = await _authService.Login(dto);
            if (usuario == null)
                return Unauthorized("Email o contraseña incorrectos.");

            return Ok(usuario);

    
        }
    }
}
