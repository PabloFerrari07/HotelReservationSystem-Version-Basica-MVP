using HotelAplication.Dtos;
using HotelAplication.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelAplication.Services
{
    public class AdminService : IAdminService
    {
        private readonly HotelContext _context;
        public AdminService(HotelContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UsuarioDto>> ObtenerUsuarios() => await _context.Usuarios.Select(x => new UsuarioDto
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            Rol = x.Rol
        }).ToListAsync();
        public async Task<UsuarioDto> ObtenerUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return null;
            };

            var usuarioDto = new UsuarioDto { Id = usuario.Id, Name = usuario.Name, Email = usuario.Email, Rol = usuario.Rol };

            return usuarioDto;

        }
        public async Task<UsuarioDto> EditarUsuario(int id, UsuarioDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return null;
            }

            usuario.Name = dto.Name;
            usuario.Email = dto.Email;
            usuario.Rol = dto.Rol;

            await _context.SaveChangesAsync();

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Name = usuario.Name,
                Email = usuario.Email,
                Rol = dto.Rol
            };

            return usuarioDto;
        }

        public async Task<UsuarioDto> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return null;

            // Verificamos si el usuario tiene reservas
            bool tieneReservas = await _context.Reservas.AnyAsync(r => r.IdUsuario == id);
            if (tieneReservas)
                throw new Exception("No se puede eliminar el usuario porque tiene reservas asociadas.");

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Name = usuario.Name,
                Email = usuario.Email,
                Rol = usuario.Rol
            };

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return usuarioDto;
        }



    }
}
