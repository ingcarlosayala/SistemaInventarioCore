using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.Models
{
    public class Inventario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Usuario es requerido")]
        [Display(Name = "Usuario")]
        public string UsuarioAplicacionId { get; set; }

        [ForeignKey("UsuarioAplicacionId")]
        public UsuarioAplicacion UsuarioAplicacion { get; set; }

        [Required(ErrorMessage = "Fecha inicial es requerido")]
        [Display(Name = "Fecha Inicial")]
        public DateTime FechaInicial { get; set; }


        [Required(ErrorMessage = "Fecha final es requerido")]
        [Display(Name = "Fecha Final")]
        public DateTime FechaFinal { get; set; }

        [Required(ErrorMessage = "Bodega es requerido")]
        [Display(Name = "Bodega")]
        public int BodegaId { get; set; }


        [ForeignKey("BodegaId")]
        public Bodega Bodega { get; set; }

        [Required(ErrorMessage = "Estado es requerido")]
        public bool Estado { get; set; }
    }
}
