using System;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SisGesVentas.Shared.Model
{
    public class Venta
    {
        [Key]
        public int VentaId { get; set; }
        public double VentaMonto { get; set; }
        public DateTime VentaFecha { get; set; } = DateTime.Now;

    }
}
