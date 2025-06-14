using HotelAplication.Dtos;
using HotelAplication.Models;
using HotelAplication.Services;
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
        public HabitacionController(IHabitacionServices habitacionServices)
        {
            _habitacionServices = habitacionServices;
        }
        [HttpGet]
        [Route("ObtenerHabitacion")]
        public async Task<IEnumerable<HabitacionDto>> Obtener() => await _habitacionServices.ObtenerHabitaciones();

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("AgregarHabitacion")]
        public async Task<ActionResult<HabitacionDto>> agregarHabitacion(HabitacionDto habitacionDto)
        {
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



    }
}
