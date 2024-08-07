using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace WebApi.Models
{
    public class Producto
    {
        public String ProductoId { get; set; }
        public String ProductoNombre { get; set; }
        public String ProductoDescripcion { get; set; }
        public double ProductoStock { get; set; }
            public Blob ProductoFoto {  get; set; }
        public String ProductoUnidad { get; set; }
        public double ProductoPrecioCompra { get; set; }
        public double ProductoPrecioVenta { get; set; }
        public double ProductoExistencias { get; set; }
        public int CategoriaId { get; set; }
        public int ProveedorId { get; set; }
    }
}
