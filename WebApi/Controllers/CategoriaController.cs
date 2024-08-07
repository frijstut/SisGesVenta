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
    public class CategoriaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
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
                await _context.Categorias.ToListAsync();
                return Ok("Buena Calidad");
            }
            catch (Exception ex)
            {
                return BadRequest($"Mala calidad - {ex.Message}");
            }
        }

        // Get all categories
        [HttpGet("Listado")]
        public async Task<ActionResult<List<Categoria>>> ListadoCategorias()
        {
            var lista = await _context.Categorias.ToListAsync();
            return Ok(lista);
        }

        // Get category by ID
        [HttpGet("ConsultarId/{id}")]
        public async Task<ActionResult<Categoria>> BuscarUnaCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound("Categoría no encontrada");
            }

            return Ok(categoria);
        }

        // Create a new category
        [HttpPost("Crear")]
        public async Task<ActionResult> CrearCategoria([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(BuscarUnaCategoria), new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error durante el proceso de almacenamiento - {ex.Message}");
            }
        }

        // Add a list of categories
        [HttpPost("AgregarListado")]
        public async Task<ActionResult> AgregarListadoCategoria([FromBody] List<Categoria> listado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var item in listado)
                {
                    if (!await _context.Categorias.AnyAsync(ob => ob.CategoriaId == item.CategoriaId))
                    {
                        _context.Categorias.Add(item);
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

        // Update an existing category
        [HttpPut("Actualizar/{id}")]
        public async Task<ActionResult> ActualizarCategoria(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbCategoria = await _context.Categorias.FindAsync(id);
            if (dbCategoria == null)
            {
                return NotFound("Categoría no encontrada");
            }

            dbCategoria.CategoriaNombre = categoria.CategoriaNombre;
            dbCategoria.CategoriaDescripcion = categoria.CategoriaDescripcion;

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

        // Delete a category by ID
        [HttpDelete("Eliminar/{id}")]
        public async Task<ActionResult> BorrarCategoria(int id)
        {
            var dbCategoria = await _context.Categorias.FindAsync(id);
            if (dbCategoria == null)
            {
                return NotFound("Categoría no encontrada");
            }

            try
            {
                _context.Categorias.Remove(dbCategoria);
                await _context.SaveChangesAsync();
                return Ok("Eliminado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar - {ex.Message}");
            }
        }
    }
}