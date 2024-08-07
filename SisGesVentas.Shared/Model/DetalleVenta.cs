using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace SisGesVentas.Shared.Model
{
    public class DetalleVenta
    {
        public int DetalleVentaId { get; set; }
        public long VentaId { get; set; }
        public String ProductoId { get; set; }
        public double CantidadVendida { get; set; }
    }
}
