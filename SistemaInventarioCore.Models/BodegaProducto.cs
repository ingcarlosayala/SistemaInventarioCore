using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.Models
{
    public class BodegaProducto
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bodega es requeriro")]
        [Display(Name = "Bodega")]
        public int BodegaId { get; set; }

        [ForeignKey("BodegaId")]
        public Bodega Bodega { get; set; }

        [Required(ErrorMessage = "Producto es requeriro")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "Cantidad es requeriro")]
        public int Cantidad { get; set; }
    }
}
