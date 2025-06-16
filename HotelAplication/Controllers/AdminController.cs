using FluentValidation;
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
    public class AdmilController : ControllerBase
    {
        private readonly HotelContext _context;
        private readonly IAdminService _adminService;
        private readonly IValidator<UsuarioDto> _validator;
        public AdmilController(HotelContext context, IAdminService adminService, IValidator<UsuarioDto> validator)
        {
            _context = context;
            _adminService = adminService;
            _validator = validator;
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("ObtenerUsuarios")]
        public async Task<IEnumerable<UsuarioDto>> mostrarUsuarios() => await _adminService.ObtenerUsuarios();

        [Authorize(Roles = "admin")]
        [HttpGet("obtenerClientes/{id}")]


        public async Task<ActionResult<UsuarioDto>> GetByID(int id)
        {
            var usuario = await _adminService.ObtenerUsuarioPorId(id);
            return usuario == null ? NotFound() : usuario;


        }
        [Authorize(Roles = "admin")]
        [HttpPut("editarClientes/{id}")]


        public async Task<ActionResult<UsuarioDto>> Editar(int id, UsuarioDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var usuario = await _adminService.EditarUsuario(id, dto);
            return usuario == null ? NotFound() : usuario;

        }

        [Authorize(Roles = "admin")]
        [HttpDelete("eliminarClientes/{id}")]
        public async Task<ActionResult<UsuarioDto>> Eliminar(int id)
        {
            var usuario = await _adminService.EliminarUsuario(id);
            return usuario == null ? NotFound() : Ok(usuario);
        }
        }
}
