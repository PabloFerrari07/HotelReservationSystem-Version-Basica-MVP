using FluentValidation;
using HotelAplication.Dtos;
using HotelAplication.Models;
using HotelAplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelAplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IValidator<CrearReservaDto> _reservaValidator;
        private readonly JwtService _jwtService;
        private readonly IHabitacionServices _habitacionServices;
        public ReservaController(IReservaService reservaService,
                               JwtService jwtService,
                               IHabitacionServices habitacionServices,
                               IValidator<CrearReservaDto> reservaValidator)
        {
            _reservaService = reservaService;
            _jwtService = jwtService;
            _habitacionServices = habitacionServices;
            _reservaValidator = reservaValidator;
        }

        [Authorize(Roles = "cliente")]
        [HttpPost]
        public async Task<ActionResult<ReservaDto>> CrearReserva(CrearReservaDto dto)
        {
            var validationResult = await _reservaValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var reserva = await _reservaService.CrearReserva(idUsuario, dto);
            return Ok(reserva);
        }
        [Authorize(Roles = "admin")]
        [HttpGet("admin/historial")]
        public async Task<ActionResult<HistorialReservasDto>> ObtenerHistorialCompleto()
        {
            var historial =await _reservaService.ObtenerHistorialCompleto();
            return Ok(historial);
        }

        [Authorize(Roles = "cliente")]
        [HttpGet("cliente/dashboard")]
        public async Task<ActionResult<DashBoardUsuarioDto>> GetDashboard()
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var dashboard = await _reservaService.ObtenerDashboard(idUsuario);

            return Ok(dashboard);
        }


        [Authorize(Roles ="cliente,admin")]
        [HttpGet("historial")]
        public async Task<ActionResult<HistorialReservasDto>> ObtenerHistorial()
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "id").Value);    
            var historial = await _reservaService.ObtenerHistorialReservas(idUsuario);

            return Ok(historial);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("dashboard/admin")]
        public async Task<ActionResult<DashboardAdminDto>> GetDashboardAdmin()
        {
            var dashboard = await _reservaService.ObtenerDashboardAdmin();
            return Ok(dashboard);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin/reservas")]
        public async Task<ActionResult<List<ReservaDto>>> ObtenerReservasAdmin([FromQuery] FiltroReservaDto filtro)
        {
            var reservas = await _reservaService.ObtenerReservasConFiltros(filtro);
            return Ok(reservas);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admin/ocupacion")]
        public async Task<ActionResult<List<OcupacionPorFechaDto>>> ObtenerOcupacionPorFecha([FromBody] FiltroOcupacionDto filtro)
        {
            var ocupacion = await _reservaService.ObtenerOcupacionPorFecha(filtro);
            return Ok(ocupacion);
        }



        [Authorize(Roles = "cliente,admin")]
        [HttpGet("mis-reservas")]
        public async Task<ActionResult<List<ReservaDto>>> ObtenerReservas()
        {
            var idUsuario = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var reservas = await _reservaService.ObtenerReservasPorUsuario(idUsuario);
            return Ok(reservas);
        }
        [Authorize(Roles = "cliente,admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaDto>> ObtenerReservaPorId(int id)
        {
            try
            {
                var reserva = await _reservaService.ObtenerReservaPorId(id);
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpPost("filtro")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<ReservaDto>>> ObtenerReservasConFiltros([FromBody] FiltroReservaDto filtro)
        {
            try
            {
                var reservas = await _reservaService.ObtenerReservasConFiltros(filtro);
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al filtrar reservas: {ex.Message}");
            }
        }

        [Authorize(Roles = "cliente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            try
            {
                int userId = ObtenerIdDesdeToken();
                await _reservaService.CancelarReservaUsuario(id, userId);
                return Ok(new { mensaje = "Reserva cancelada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("admin/eliminarReserva/{id}")]
        public async Task<IActionResult> CancelarReservaAdmin(int id)
        {
            try
            {
                await _reservaService.CancelarReservaAdmin(id);
                return Ok(new { mensaje = "Reserva eliminada por el administrador." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        private int ObtenerIdDesdeToken()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (claim == null)
                throw new Exception("No se pudo obtener el ID del token");

            return int.Parse(claim.Value);
        }


    }

}
