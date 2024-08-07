using System.ComponentModel.DataAnnotations;

namespace SisGesVentas.Shared.Model
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }


        [StringLength(100)]
        public string UsuarioNombre { get; set; }


        [EmailAddress]
        public string UsuarioUser { get; set; }


        [StringLength(20)]  // Adjust length as needed for hashed passwords
        public string UsuarioPassword { get; set; }
    }
}
