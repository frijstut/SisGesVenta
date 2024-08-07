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
    public class ProductoController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
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
                var Producto = await _context.Productos.ToListAsync();
                return Ok("Buena Calidad");
            }
            catch (Exception ex)
            {
                return BadRequest($"Mala calidad - {ex.Message}");
            }
        }

        // Obtener todos los Producto
        [HttpGet("Listado")]
        public async Task<ActionResult<List<Producto>>> GetProducto()
        {
            var lista = await _context.Productos.ToListAsync();
            return Ok(lista);
        }

        // Obtener un Producto por ID
        [HttpGet("ConsultarId/{id}")]
        public async Task<ActionResult<Producto>> GetSingleProducto(int id)
        {
            var objeto = await _context.Productos.FindAsync(id);
            if (objeto == null)
            {
                return NotFound("Producto no encontrado");
            }

            return Ok(objeto);
        }

        // Crear un nuevo Producto
        [HttpPost("Crear")]
        public async Task<ActionResult> CreateProducto([FromBody] Producto objeto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Productos.Add(objeto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSingleProducto), new { id = objeto.ProductoId }, objeto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error durante el proceso de almacenamiento - {ex.Message}");
            }
        }

        // Agregar una lista de Producto
        [HttpPost("AgregarListado")]
        public async Task<ActionResult> AgregarListadoProducto([FromBody] List<Producto> listado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var item in listado)
                {
                    if (!await _context.Productos.AnyAsync(ob => ob.ProductoId == item.ProductoId))
                    {
                        _context.Productos.Add(item);
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

        // Actualizar un Producto
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> UpdateProducto(string id, [FromBody] Producto objeto)
        {
            if (id != objeto.ProductoId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbObjeto = await _context.Productos.FindAsync(id);
            if (dbObjeto == null)
            {
                return NotFound("Producto no encontrado");
            }

            // Actualizar las propiedades necesarias
            dbObjeto.ProductoId = objeto.ProductoId; // Actualiza con las propiedades correspondientes
           // dbObjeto.Property2 = objeto.Property2;
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

        // Eliminar un Producto por ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> DeleteProducto(int id)
        {
            var dbObjeto = await _context.Productos.FindAsync(id);
            if (dbObjeto == null)
            {
                return NotFound("Producto no encontrado");
            }

            try
            {
                _context.Productos.Remove(dbObjeto);
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
