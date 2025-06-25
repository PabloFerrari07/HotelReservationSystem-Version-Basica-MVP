using HotelAplication.Dtos;
using HotelAplication.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<ReservaDto> ObtenerReservaPorId(int idReserva)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Habitacion)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == idReserva);

            if(reserva == null)
            {
                throw new Exception($"No se encontró una reserva con el ID {idReserva}.");
            }
            return new ReservaDto
            {
                Id = reserva.Id,
                NombreUsuario = reserva.Usuario.Name,
                NumeroHabitacion = reserva.Habitacion.Numero,
                FechaEntrada = reserva.FechaEntrada,
                FechaSalida = reserva.FechaSalida,
                Estado = reserva.Estado

            };
        }

        public async Task<ReservaDto> CrearReserva(int idUsuario, CrearReservaDto dto)
        {
            var habitacion = await _context.Habitaciones.FindAsync(dto.IdHabitacion);
            if (habitacion == null)
            {
                throw new Exception($"La habitación con ID {dto.IdHabitacion} no existe.");
            }

            var hoy = DateTime.Today;
            var maxFecha = hoy.AddYears(1);

            if (dto.FechaEntrada < hoy)
                throw new Exception("La fecha de entrada no puede ser anterior a hoy.");

            if (dto.FechaEntrada > maxFecha)
                throw new Exception("No se pueden hacer reservas con más de un año de anticipación.");

            if (dto.FechaSalida <= dto.FechaEntrada)
                throw new Exception("La fecha de salida debe ser posterior a la fecha de entrada.");

            if (dto.FechaSalida > maxFecha)
                throw new Exception("La fecha de salida no puede superar un año desde hoy.");


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

        public async Task CancelarReservaUsuario(int idReserva, int idUsuario)
        {
            var reserva = await _context.Reservas
     .FirstOrDefaultAsync(r => r.Id == idReserva && r.IdUsuario == idUsuario);

            if (reserva == null)          
                throw new Exception("Reserva no encontrada o Acceso denegado");
            

            reserva.Estado = "cancelada";

            await _context.SaveChangesAsync();
        }

        public async Task CancelarReservaAdmin(int idReserva)
        {

            var reserva = await _context.Reservas.FindAsync(idReserva);
            if (reserva == null)
                throw new Exception("Reserva no encontrada o Acceso denegado");

            reserva.Estado = "cancelada";
            await _context.SaveChangesAsync();

        }


            public async Task<HistorialReservasDto> ObtenerHistorialReservas(int idUsuario)
        {
            await ActualizacionEstadoReservas();
            var reservas = await _context.Reservas
                                    .Include(r => r.Usuario)
                                    .Include(r => r.Habitacion)
                                    .Where(r => r.IdUsuario == idUsuario).ToListAsync();

            var activas = reservas.Where(r => r.Estado == "activa").Select(MapearDto).ToList();
            var cancelada = reservas.Where(r => r.Estado == "cancelada").Select(MapearDto).ToList();
            var finalizada = reservas.Where(r => r.Estado == "finalizada").Select(MapearDto).ToList();

            return new HistorialReservasDto
            {
                Activas = activas,
                Canceladas = cancelada,
                Finalizadas = finalizada
            };


        }

        public async Task<HistorialReservasDto> ObtenerHistorialCompleto()
        {
            await ActualizacionEstadoReservas();

            var reservas = await _context.Reservas
                                    .Include(r => r.Usuario)
                                    .Include(r => r.Habitacion).ToListAsync();

            var activas = reservas.Where(r => r.Estado == "activa").Select(MapearDto).ToList();
            var cancelada = reservas.Where(r => r.Estado == "cancelada").Select(MapearDto).ToList();
            var finalizada = reservas.Where(r => r.Estado == "finalizada").Select(MapearDto).ToList();

            return new HistorialReservasDto
            {
                Activas = activas,
                Canceladas = cancelada,
                Finalizadas = finalizada
            };
        }

        public async Task<DashBoardUsuarioDto> ObtenerDashboard(int idUsuario)
        {
            await ActualizacionEstadoReservas();

            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            var reservas = await _context.Reservas
                                         .Include(r => r.Habitacion)
                                         .Where(r => r.IdUsuario == idUsuario)
                                         .ToListAsync();

            var hoy = DateTime.Today;

            var proximasReservas = reservas
                .Where(r => r.FechaEntrada > hoy && r.Estado == "activa")
                .Where(r => r.Habitacion != null)
                .Select(r =>
                {
                    try
                    {
                        return MapearDto(r);
                    }
                    catch
                    {
                   
                        return null;
                    }
                })
                .Where(r => r != null)
                .ToList();

            return new DashBoardUsuarioDto
            {
                Nombre = usuario.Name,
                Email = usuario.Email,
                TotalReservas = reservas.Count,
                Activas = reservas.Count(r => r.Estado == "activa"),
                Canceladas = reservas.Count(r => r.Estado == "cancelada"),
                Finalizadas = reservas.Count(r => r.Estado == "finalizada"),
                ProximasReservas = proximasReservas
            };
        }



        public async Task<DashboardAdminDto> ObtenerDashboardAdmin()
        {
            await ActualizacionEstadoReservas();

            var totalUsuarios = await _context.Usuarios.CountAsync();
            var totalReservas = await _context.Reservas.CountAsync();
            var reservasActivas = await _context.Reservas.CountAsync(r => r.Estado == "activa");
            var reservasCanceladas = await _context.Reservas.CountAsync(r => r.Estado == "cancelada");
            var reservasFinalizadas = await _context.Reservas.CountAsync(r => r.Estado == "finalizada");
            var habitacionesDisponible = await _context.Habitaciones.CountAsync(h => h.Disponible);
            var habitacionesOcupadas = await _context.Habitaciones.CountAsync(h => !h.Disponible);

            return new DashboardAdminDto
            {
                TotalUsuarios = totalUsuarios,
                TotalReservas = totalReservas,
                ReservasActivas = reservasActivas,
                ReservasCanceladas = reservasCanceladas,
                ReservasFinalizadas = reservasFinalizadas,
                HabitacionesDisponibles = habitacionesDisponible,
                HabitacionesOcupadas = habitacionesOcupadas
            };
        }

        public async Task<List<ReservaDto>> ObtenerReservasConFiltros(FiltroReservaDto filtro)
        {
            var query = _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Estado))
            {
                query = query.Where(r => r.Estado == filtro.Estado);
            }

            if (filtro.IdUsuario.HasValue)
            {
                query = query.Where(r => r.IdUsuario == filtro.IdUsuario.Value);
            }

            if (filtro.Desde.HasValue && filtro.Hasta.HasValue)
            {
                var desdeFecha = filtro.Desde.Value.Date;
                var hastaFecha = filtro.Hasta.Value.Date;

                query = query.Where(r =>
                    r.FechaEntrada.Date <= hastaFecha &&
                    r.FechaSalida.Date >= desdeFecha
                );
            }

            var reservas = await query.ToListAsync();

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

        public async Task<List<OcupacionPorFechaDto>> ObtenerOcupacionPorFecha(FiltroOcupacionDto filtro)
        {
            var resultado = new List<OcupacionPorFechaDto>();

            for(DateTime fecha = filtro.FechaDesde.Date; fecha <= filtro.FechaHasta.Date; fecha = fecha.AddDays(1))
            {
                var ocupadasEseDia = await _context.Reservas
                    .Where(r => r.Estado == "activa" && fecha >= r.FechaEntrada.Date && fecha <= r.FechaSalida.Date).CountAsync();

                resultado.Add(new OcupacionPorFechaDto
                {
                    Fecha = fecha,
                    HabitacionesOcupadas = ocupadasEseDia
                });
            }

            return resultado;
        }


        private async Task ActualizacionEstadoReservas()
        {
            var reservas = await _context.Reservas
                                         .Where(r => r.Estado == "activa" && r.FechaSalida < DateTime.Today).ToListAsync();

            foreach (var r in reservas)
            {
                r.Estado = "finalizada";
            }
            await _context.SaveChangesAsync();
        }

        private ReservaDto MapearDto(Reserva r)
        {
            return new ReservaDto
            {
                Id = r.Id,
                NombreUsuario = r.Usuario?.Name,
                NumeroHabitacion = r.Habitacion?.Numero ?? 0,
                FechaEntrada = r.FechaEntrada,
                FechaSalida = r.FechaSalida,
                Estado = r.Estado
            };
        }


    }

}
