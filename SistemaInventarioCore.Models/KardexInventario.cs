using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.Models
{
    public class KardexInventario
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bodega Producto es requerido")]
        [Display(Name = "Bodega Producto")]
        public int BodegaProductoId { get; set; }

        [ForeignKey("BodegaProductoId")]
        public BodegaProducto BodegaProducto { get; set; }

        [Required(ErrorMessage = "Tipo es requerido")]
        [MaxLength(100)]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Detalle es requerido")]
        public string Detalle { get; set; }

        [Required(ErrorMessage = "Stock Anterior es requerido")]
        [Display(Name = "Stock Inicial")]
        public int StockAnterior { get; set; }

        [Required(ErrorMessage = "Cantidad es requerido")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Costo es requerido")]
        public double Costo { get; set; }

        [Required(ErrorMessage = "Stock es requerido")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Total es requerido")]
        public double Total { get; set; }

        [Required(ErrorMessage = "Usuario es requerido")]
        [Display(Name = "Usuario")]
        public string UsuarioAplicacionId { get; set; }

        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
    }
}
