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
    public class DetalleVentaController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public DetalleVentaController(ApplicationDbContext context)
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
                var especialidades = await _context.DetalleVentas.ToListAsync();
                return Ok("Buena Calidad");
            }
            catch (Exception ex)
            {
                return BadRequest($"Mala calidad - {ex.Message}");
            }
        }

        // Obtener todos los DetalleVentas
        [HttpGet("Listado")]
        public async Task<ActionResult<List<DetalleVenta>>> GetDetalleVentas()
        {
            var lista = await _context.DetalleVentas.ToListAsync();
            return Ok(lista);
        }

        // Obtener un DetalleVenta por ID
        [HttpGet("ConsultarId/{id}")]
        public async Task<ActionResult<DetalleVenta>> GetSingleDetalleVenta(int id)
        {
            var objeto = await _context.DetalleVentas.FindAsync(id);
            if (objeto == null)
            {
                return NotFound("DetalleVenta no encontrado");
            }

            return Ok(objeto);
        }

        // Crear un nuevo DetalleVenta
        [HttpPost("Crear")]
        public async Task<ActionResult> CreateDetalleVenta([FromBody] DetalleVenta objeto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.DetalleVentas.Add(objeto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSingleDetalleVenta), new { id = objeto.DetalleVentaId }, objeto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error durante el proceso de almacenamiento - {ex.Message}");
            }
        }

        // Agregar una lista de DetalleVentas
        [HttpPost("AgregarListado")]
        public async Task<ActionResult> AgregarListadoDetalleVentas([FromBody] List<DetalleVenta> listado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var item in listado)
                {
                    if (!await _context.DetalleVentas.AnyAsync(ob => ob.DetalleVentaId == item.DetalleVentaId))
                    {
                        _context.DetalleVentas.Add(item);
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

        // Actualizar un DetalleVenta
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> UpdateDetalleVenta(int id, [FromBody] DetalleVenta objeto)
        {
            if (id != objeto.DetalleVentaId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbObjeto = await _context.DetalleVentas.FindAsync(id);
            if (dbObjeto == null)
            {
                return NotFound("DetalleVenta no encontrado");
            }

            // Actualizar las propiedades necesarias
            dbObjeto.DetalleVentaId = objeto.DetalleVentaId; // Actualiza con las propiedades correspondientes
         //   dbObjeto.Property2 = objeto.Property2;
            // Actualiza otras propiedades según sea necesario

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

        // Eliminar un DetalleVenta por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> DeleteDetalleVenta(int id)
        {
            var dbObjeto = await _context.DetalleVentas.FindAsync(id);
            if (dbObjeto == null)
            {
                return NotFound("DetalleVenta no encontrado");
            }

            try
            {
                _context.DetalleVentas.Remove(dbObjeto);
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
