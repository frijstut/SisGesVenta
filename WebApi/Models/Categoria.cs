using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
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
