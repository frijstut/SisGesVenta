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
    public class VentaController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public VentaController(ApplicationDbContext context)
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
                var Ventas = await _context.Ventas.ToListAsync();
                return Ok("Buena Calidad");
            }
            catch (Exception ex)
            {
                return BadRequest($"Mala calidad - {ex.Message}");
            }
        }

        // Obtener todos los Venta
        [HttpGet("Listado")]
        public async Task<ActionResult<List<Venta>>> GetVentas()
        {
            var lista = await _context.Ventas.ToListAsync();
            return Ok(lista);
        }

        // Obtener un Venta por ID
        [HttpGet("ConsultarId/{id}")]
        public async Task<ActionResult<Venta>> GetSingleVenta(int id)
        {
            var objeto = await _context.Ventas.FindAsync(id);
            if (objeto == null)
            {
                return NotFound("Venta no encontrado");
            }

            return Ok(objeto);
        }

        // Crear un nuevo Venta
        [HttpPost("Crear")]
        public async Task<ActionResult> CreateVenta([FromBody] Venta objeto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Ventas.Add(objeto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSingleVenta), new { id = objeto.VentaId }, objeto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error durante el proceso de almacenamiento - {ex.Message}");
            }
        }

        // Agregar una lista de Venta
        [HttpPost("AgregarListado")]
        public async Task<ActionResult> AgregarListadoVentas([FromBody] List<Venta> listado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var item in listado)
                {
                    if (!await _context.Ventas.AnyAsync(ob => ob.VentaId == item.VentaId))
                    {
                        _context.Ventas.Add(item);
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

        // Actualizar un Venta
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> UpdateVenta(int id, [FromBody] Venta objeto)
        {
            if (id != objeto.VentaId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbObjeto = await _context.Ventas.FindAsync(id);
            if (dbObjeto == null)
            {
                return NotFound("Venta no encontrado");
            }

            // Actualizar las propiedades necesarias
            dbObjeto.VentaId = objeto.VentaId;
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

        // Eliminar un Venta por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> DeleteVenta(int id)
        {
            var dbObjeto = await _context.Ventas.FindAsync(id);
            if (dbObjeto == null)
            {
                return NotFound("Venta no encontrado");
            }

            try
            {
                _context.Ventas.Remove(dbObjeto);
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
