using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebApi.Models
{
    public class DetalleVenta
    {
        public int DetalleVentaId { get; set; }
        public long VentaId { get; set; }
        public String ProductoId { get; set; }
        public double CantidadVendida { get; set; }
    }
}
