using FluentValidation;
using HotelAplication.Dtos;
using HotelAplication.Models;
using HotelAplication.Services;
using HotelAplication.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionController : ControllerBase
    {
        private readonly IHabitacionServices _habitacionServices;
        private readonly IValidator<HabitacionDto> _HabitacionValidator;
        public HabitacionController(IHabitacionServices habitacionServices, IValidator<HabitacionDto> habitacionValidator)
        {
            _habitacionServices = habitacionServices;
            _HabitacionValidator = habitacionValidator;
        }
        [HttpGet]
        [Route("ObtenerHabitacion")]
        public async Task<IEnumerable<HabitacionDto>> Obtener() => await _habitacionServices.ObtenerHabitaciones();
        [Authorize(Roles = "cliente")]
        [HttpGet]
        [Route("HabitacionesDisponibles")]
        public async Task<ActionResult<List<HabitacionDto>>> ObtenerHabitacionesDisponibles(DateTime fechaInicio,DateTime fechaFin)
        {
            if(fechaInicio >= fechaFin)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            var disponibles = await _habitacionServices.ObtenerHabitacionesDisponibles(fechaInicio, fechaFin);
            return Ok(disponibles);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("AgregarHabitacion")]
        public async Task<ActionResult<HabitacionDto>> agregarHabitacion(HabitacionDto habitacionDto)
        {
            var validationResult = await _HabitacionValidator.ValidateAsync(habitacionDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var habitacion = await _habitacionServices.AgregarHabitacion(habitacionDto); 

            return Ok(habitacion);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("ObtenerHabitacion/{id}")]
        public async Task<ActionResult<HabitacionDto>> ObtenerHabitacionId(int id)
        {
           var habitacionEncontrada = await _habitacionServices.ObtenerHabitacionPorNumero(id);
            return habitacionEncontrada == null ? NotFound() : habitacionEncontrada;
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("EditarHabitacion/{id}")]
        public async Task<ActionResult<HabitacionDto>>editarHabitacion(int id,HabitacionDto habitacionDto)
        {
            var habitacionModificada = await _habitacionServices.EditarHabitacion(id, habitacionDto);
            return habitacionModificada ==null ? NotFound() : Ok(habitacionModificada);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("EliminarHabitacion/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _habitacionServices.EliminarHabitacion(id);
                return Ok(new { mensaje = "Habitación eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }



    }
}
