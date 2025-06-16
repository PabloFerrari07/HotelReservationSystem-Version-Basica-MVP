using HotelAplication.Dtos;
using HotelAplication.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelAplication.Services
{
    public class ReservaService : IReservaService
    {
        private readonly HotelContext _context;

        public ReservaService(HotelContext context)
        {
            _context = context;
        }

        public async Task<List<ReservaDto>> ObtenerReservasPorUsuario(int idUsuario)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Habitacion)
                .Include(r => r.Usuario)
                .Where(r => r.IdUsuario == idUsuario)
                .ToListAsync();

            return reservas.Select(r => new ReservaDto
            {
                Id = r.Id,
                NombreUsuario = r.Usuario.Name,
                NumeroHabitacion = r.Habitacion.Numero,
                FechaEntrada = r.FechaEntrada,
                FechaSalida = r.FechaSalida,
                Estado = r.Estado
            }).ToList();
        }

        public async Task<ReservaDto> CrearReserva(int idUsuario, CrearReservaDto dto)
        {
            var habitacion = await _context.Habitaciones.FindAsync(dto.IdHabitacion);
            if (habitacion == null)
            {
                throw new Exception($"La habitación con ID {dto.IdHabitacion} no existe.");
            }

      
            bool haySolapamiento = await _context.Reservas
                .AnyAsync(r =>
                    r.IdHabitacion == dto.IdHabitacion &&
                    r.Estado == "activa" &&
                    dto.FechaEntrada < r.FechaSalida &&
                    dto.FechaSalida > r.FechaEntrada
                );

            if (haySolapamiento)
            {
                throw new Exception("Ya existe una reserva activa para esta habitación en el rango de fechas seleccionado.");
            }

            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
            {
                throw new Exception($"El usuario con ID {idUsuario} no existe.");
            }

            var reserva = new Reserva
            {
                IdUsuario = idUsuario,
                IdHabitacion = dto.IdHabitacion,
                FechaEntrada = dto.FechaEntrada,
                FechaSalida = dto.FechaSalida,
                Estado = "activa"
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return new ReservaDto
            {
                Id = reserva.Id,
                NombreUsuario = usuario.Name,
                NumeroHabitacion = habitacion.Numero,
                FechaEntrada = reserva.FechaEntrada,
                FechaSalida = reserva.FechaSalida,
                Estado = reserva.Estado
            };
        }


        public async Task CancelarReserva(int idReserva, int idUsuario)
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.Id == idReserva && r.IdUsuario == idUsuario);

            if (reserva == null)
                throw new Exception("Reserva no encontrada o acceso denegado.");

            reserva.Estado = "cancelada";
            await _context.SaveChangesAsync();
        }
    }

}
