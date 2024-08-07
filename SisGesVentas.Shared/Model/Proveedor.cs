using System;
using System.ComponentModel.DataAnnotations;

namespace SisGesVentas.Shared.Model
{
    public class Proveedor
    {
        public int ProveedorId { get; set; }
        public String ProveedorNombre { get; set; }
        public String ProveedorDireccion { get; set; }
        public String ProveedorTelefono { get; set; }
        public String ProveedorEmail { get; set; }
        public String ProveedorContacto { get; set; }
    }
}
