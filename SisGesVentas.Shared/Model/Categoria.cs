using System.ComponentModel.DataAnnotations;

namespace SisGesVentas.Shared.Model
{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }
        [Required]
        public String CategoriaNombre { get; set; }
        [Required]
        public String CategoriaDescripcion { get; set; }
    }
}
