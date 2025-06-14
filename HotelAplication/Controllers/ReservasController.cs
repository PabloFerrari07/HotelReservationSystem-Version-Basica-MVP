using HotelAplication.Dtos;
using HotelAplication.Models;
using HotelAplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelAplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly JwtService _jwtService;
        private readonly IHabitacionServices _habitacionServices;
        public ReservaController(IReservaService reservaService, JwtService jwtService, IHabitacionServices habitacionServices)
        {
            _reservaService = reservaService;
            _jwtService = jwtService;
            _habitacionServices = habitacionServices;
        }

        [Authorize(Roles = "cliente")]
        [HttpPost]
        public async Task<ActionResult<ReservaDto>> CrearReserva(CrearReservaDto dto)
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var reserva = await _reservaService.CrearReserva(idUsuario, dto);
            return Ok(reserva);
        }

        [Authorize(Roles = "cliente")]
        [HttpGet("mis-reservas")]
        public async Task<ActionResult<List<ReservaDto>>> ObtenerReservas()
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var reservas = await _reservaService.ObtenerReservasPorUsuario(idUsuario);
            return Ok(reservas);
        }

        [Authorize(Roles = "cliente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            await _reservaService.CancelarReserva(id, idUsuario);
            return NoContent();
        }
    }

}
