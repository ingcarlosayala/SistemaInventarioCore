using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.Models
{
    public class InventarioDetalle
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Inventario es requerido")]
        [Display(Name = "Inventario")]
        public int InventarioId { get; set; }

        [ForeignKey("InventarioId")]
        public Inventario Inventario { get; set; }

        [Required(ErrorMessage = "Producto es requerido")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "Stock Anterior es requerido")]
        [Display(Name = "Stock Anterior")]
        public int StockAnterior { get; set; }

        [Required(ErrorMessage = "Cantidad es requerido")]
        public int Cantidad { get; set; }
    }
}
