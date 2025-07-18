﻿using FluentValidation;
using HotelAplication.Dtos;
using HotelAplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
    
            if (!habitacionDto.Numero.HasValue)
                throw new Exception("El número de habitación es obligatorio.");
            if (!habitacionDto.Disponible.HasValue)
                throw new Exception("La disponibilidad es obligatoria.");
            if (!habitacionDto.PrecioPorNoche.HasValue)
                throw new Exception("El precio por noche es obligatorio.");
            if (string.IsNullOrWhiteSpace(habitacionDto.Tipo))
                throw new Exception("El tipo de habitación es obligatorio.");
            bool existeNumero = await _context.Habitaciones.AnyAsync(h => h.Numero == habitacionDto.Numero.Value);
            if (existeNumero)
                throw new Exception($"Ya existe una habitación con el número {habitacionDto.Numero}.");

            var habitacion = new Habitacion
            {
                Numero = habitacionDto.Numero.Value,
                Disponible = habitacionDto.Disponible.Value,
                PrecioPorNoche = habitacionDto.PrecioPorNoche.Value,
                Tipo = habitacionDto.Tipo
            };

            await _context.Habitaciones.AddAsync(habitacion);
            await _context.SaveChangesAsync();

            return new HabitacionDto
            {
                Id = habitacion.Id,
                Numero = habitacion.Numero,
                Disponible = habitacion.Disponible,
                PrecioPorNoche = habitacion.PrecioPorNoche,
                Tipo = habitacion.Tipo
            };
        }



        public async Task<HabitacionDto> EditarHabitacion(int id, HabitacionDto habitacionDto)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null)
                return null;

            if (!habitacionDto.Numero.HasValue || !habitacionDto.Disponible.HasValue ||
                !habitacionDto.PrecioPorNoche.HasValue || string.IsNullOrWhiteSpace(habitacionDto.Tipo))
                throw new Exception("Todos los campos son obligatorios.");

            habitacion.Numero = habitacionDto.Numero.Value;
            habitacion.Disponible = habitacionDto.Disponible.Value;
            habitacion.PrecioPorNoche = habitacionDto.PrecioPorNoche.Value;
            habitacion.Tipo = habitacionDto.Tipo;

            await _context.SaveChangesAsync();

            return new HabitacionDto
            {
                Id = habitacion.Id,
                Numero = habitacion.Numero,
                Disponible = habitacion.Disponible,
                PrecioPorNoche = habitacion.PrecioPorNoche,
                Tipo = habitacion.Tipo
            };
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

        public async Task EliminarHabitacion(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);

            if (habitacion == null)
                throw new Exception("Habitación no encontrada");

            var tieneReservas = await _context.Reservas.AnyAsync(r => r.IdHabitacion == id);

            if (tieneReservas)
                throw new Exception("No se puede eliminar la habitación porque tiene reservas asociadas.");

            _context.Habitaciones.Remove(habitacion);
            await _context.SaveChangesAsync();
        }


        public async Task<List<HabitacionDto>> ObtenerHabitacionesDisponibles(DateTime fechaInicio, DateTime fechaFin)
        {
            var habitacionesOcupadas = await _context.Reservas
                .Where(r => r.FechaEntrada < fechaFin && r.FechaSalida > fechaInicio)
                .Select(r => r.IdHabitacion) 
                .ToListAsync();

            var habitacionesDisponibles = await _context.Habitaciones
                .Where(h => !habitacionesOcupadas.Contains(h.Id))
                .ToListAsync();

            return habitacionesDisponibles.Select(h => new HabitacionDto
            {
                Id = h.Id,
                Numero = h.Numero,
                Tipo = h.Tipo,
                PrecioPorNoche = h.PrecioPorNoche,
                Disponible = h.Disponible
            }).ToList();
        }
    }
}
