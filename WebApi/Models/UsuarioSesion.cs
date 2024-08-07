using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UsuarioSesion
    {
        [Required]
        [StringLength(100)]
        public string UsuarioNombres { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string UsuarioCorreo { get; set; }

        [Required]
        [StringLength(50)]
        public string UsuarioRol { get; set; }
    }
}
