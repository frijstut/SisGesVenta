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
    public class ProveedorController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public ProveedorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Estado de conexión con el servidor
        [HttpGet("ConexionServidor")]
        public ActionResult<string> GetConexionServidor()
        {
            return Ok("Conectado");
        }

        // Estado de conexión con la base de datos
        [HttpGet("ConexionDB")]
        public async Task<ActionResult<string>> GetConexionDB()
        {
            try
            {
                // Verifica la conexión a la base de datos
                var Proveedor = await _context.Proveedors.ToListAsync();
                return Ok("Buena Calidad");
            }
            catch (Exception ex)
            {
                return BadRequest($"Mala calidad - {ex.Message}");
            }
        }

        // Obtener todos los Proveedor
        [HttpGet("Listado")]
        public async Task<ActionResult<List<Proveedor>>> GetProveedors()
        {
            var lista = await _context.Proveedors.ToListAsync();
            return Ok(lista);
        }

        // Obtener un Proveedor por ID
        [HttpGet("ConsultarId/{id}")]
        public async Task<ActionResult<Proveedor>> GetSingleProveedor(int id)
        {
            var objeto = await _context.Proveedors.FindAsync(id);
            if (objeto == null)
            {
                return NotFound("Proveedor no encontrado");
            }

            return Ok(objeto);
        }

        // Crear un nuevo Proveedor
        [HttpPost("Crear")]
        public async Task<ActionResult> CreateProveedor([FromBody] Proveedor objeto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Proveedors.Add(objeto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSingleProveedor), new { id = objeto.ProveedorId }, objeto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error durante el proceso de almacenamiento - {ex.Message}");
            }
        }

        // Agregar una lista de Proveedor
        [HttpPost("AgregarListado")]
        public async Task<ActionResult> AgregarListadoProveedors([FromBody] List<Proveedor> listado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var item in listado)
                {
                    if (!await _context.Proveedors.AnyAsync(ob => ob.ProveedorId == item.ProveedorId))
                    {
                        _context.Proveedors.Add(item);
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

        // Actualizar un Proveedor
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> UpdateProveedor(int id, [FromBody] Proveedor objeto)
        {
            if (id != objeto.ProveedorId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbObjeto = await _context.Proveedors.FindAsync(id);
            if (dbObjeto == null)
            {
                return NotFound("Proveedor no encontrado");
            }

            // Actualizar las propiedades necesarias
            dbObjeto.ProveedorId = objeto.ProveedorId;
          //  dbObjeto.Descripcion = objeto.Descripcion;
            // Actualizar otras propiedades según sea necesario

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

        // Eliminar un Proveedor por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> DeleteProveedor(int id)
        {
            var dbObjeto = await _context.Proveedors.FindAsync(id);
            if (dbObjeto == null)
            {
                return NotFound("Proveedor no encontrado");
            }

            try
            {
                _context.Proveedors.Remove(dbObjeto);
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
