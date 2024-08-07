using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Endpoint to check server connection
        [HttpGet("ConexionServidor")]
        public ActionResult<string> GetConexionServidor()
        {
            return Ok("Conectado");
        }

        // Endpoint to check database connection
        [HttpGet("ConexionDB")]
        public async Task<ActionResult<string>> GetConexionDB()
        {
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                return Ok("Buena Calidad");
            }
            catch (Exception ex)
            {
                return BadRequest($"Mala calidad - {ex.Message}");
            }
        }

        // Get all Usuarios
        [HttpGet("Listado")]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            var lista = await _context.Usuarios.ToListAsync();
            return Ok(lista);
        }

        // Get Usuario by ID
        [HttpGet("ConsultarId/{id}")]
        public async Task<ActionResult<Usuario>> GetSingleUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(usuario);
        }

        // Create a new Usuario
        [HttpPost("Crear")]
        public async Task<ActionResult> CreateUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState + "Prueba de que no llega los datos");
            }

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSingleUsuario), new { id = usuario.UsuarioId }, usuario);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error durante el proceso de almacenamiento - {ex.Message}");
            }
        }

        // Add a list of Usuarios
        [HttpPost("AgregarListado")]
        public async Task<ActionResult> AddUsuarioList([FromBody] List<Usuario> listado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var item in listado)
                {
                    if (!await _context.Usuarios.AnyAsync(ob => ob.UsuarioNombre == item.UsuarioNombre))
                    {
                        _context.Usuarios.Add(item);
                    }
                }
                await _context.SaveChangesAsync();
                return Ok("Listado agregado con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error durante el proceso de almacenamiento - {ex.Message}");
            }
        }

        // Update an existing Usuario
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.UsuarioId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUsuario = await _context.Usuarios.FindAsync(id);
            if (dbUsuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            dbUsuario.UsuarioNombre = usuario.UsuarioNombre; // Update other properties as needed

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar - {ex.Message}");
            }
        }

        // Delete a Usuario by ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            var dbUsuario = await _context.Usuarios.FindAsync(id);
            if (dbUsuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            try
            {
                _context.Usuarios.Remove(dbUsuario);
                await _context.SaveChangesAsync();
                return Ok("Eliminado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"No fue posible eliminar el objeto - {ex.Message}");
            }
        }
    }
}
