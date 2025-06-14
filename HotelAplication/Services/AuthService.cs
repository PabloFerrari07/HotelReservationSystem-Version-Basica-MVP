using HotelAplication.Dtos;
using HotelAplication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly HotelContext _context;
        private readonly JwtService _jwtService;
        public AuthService(HotelContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;

        }

        public async Task<UsuarioDto> RegistrarUsuario(RegistroDto dto)
        {

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);


            var usuario = new Usuario
            {
                Name = dto.Nombre,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Rol = dto.Rol

            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var usuarioDto =  new UsuarioDto
            {
                Name = usuario.Name,
                Email = usuario.Email,
                Rol = usuario.Rol
            };



            return usuarioDto;
        }

        public async Task<UsuarioDto?> Login(LoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null)
                return null;

            bool passwordValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);

            if (!passwordValida)
            {
                return null;
            }

            string token = _jwtService.GenerarToken(usuario);

            return new UsuarioDto
            {
                Name = usuario.Name,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Token = token
            };
        }


    }
}
