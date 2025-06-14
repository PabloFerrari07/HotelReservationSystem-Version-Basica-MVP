using HotelAplication.Dtos;
using HotelAplication.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelAplication.Services
{
    public class HabitacionService : IHabitacionServices
    {
        private readonly HotelContext _context;
        public HabitacionService(HotelContext context)
        {

            _context = context;

        }
        public async Task<HabitacionDto> AgregarHabitacion(HabitacionDto habitacionDto)
        {
            var habitacion = new Habitacion
            {
                Id = habitacionDto.Id,
                Numero = habitacionDto.Numero,
                Disponible = habitacionDto.Disponible,
                PrecioPorNoche = habitacionDto.PrecioPorNoche,
                Tipo = habitacionDto.Tipo,

            };

            await _context.Habitaciones.AddAsync(habitacion);
            await _context.SaveChangesAsync();

            var habitacionCargada = new HabitacionDto
            {
                Id = habitacion.Id,
                Numero = habitacion.Numero,
                Disponible = habitacion.Disponible,
                PrecioPorNoche = habitacion.PrecioPorNoche,
                Tipo = habitacion.Tipo,

            };

            return habitacionCargada;
        }

        public async Task<HabitacionDto> EditarHabitacion(int id, HabitacionDto habitacionDto)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null)
            {
                return null;
            }

            habitacion.Numero = habitacionDto.Numero;
            habitacion.Disponible = habitacionDto.Disponible;
            habitacion.PrecioPorNoche = habitacionDto.PrecioPorNoche;
            habitacion.Tipo = habitacionDto.Tipo;

            await _context.SaveChangesAsync();

            var habitacionModificada = new HabitacionDto
            {
                Id = habitacion.Id,
                Numero = habitacion.Numero,
                Disponible = habitacion.Disponible,
                PrecioPorNoche = habitacion.PrecioPorNoche,
                Tipo = habitacion.Tipo
            };

            return habitacionModificada;
        }

        public async Task<IEnumerable<HabitacionDto>> ObtenerHabitaciones() => await _context.Habitaciones.Select(x => new HabitacionDto { Id = x.Id, Disponible = x.Disponible, Numero = x.Numero, PrecioPorNoche = x.PrecioPorNoche, Tipo = x.Tipo }).ToListAsync();

        public async Task<HabitacionDto> ObtenerHabitacionPorNumero(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null)
            {
                return null;
            }

            var habitacionEncontrada = new HabitacionDto
            {
                Id = id,
                Numero = habitacion.Numero,
                Disponible = habitacion.Disponible,
                PrecioPorNoche = habitacion.PrecioPorNoche,
                Tipo = habitacion.Tipo

            };

            return habitacionEncontrada;
        }
    }
}
