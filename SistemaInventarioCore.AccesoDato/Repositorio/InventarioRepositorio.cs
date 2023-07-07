using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioCore.AccesoDato.Data;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using SistemaInventarioCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.AccesoDato.Repositorio
{
    public class InventarioRepositorio : Repositorio<Inventario>, IInventarioRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public InventarioRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(Inventario inventario)
        {
            var item = await dbContext.Inventario.FirstOrDefaultAsync(i => i.Id == inventario.Id);

            if (item != null)
            {
                item.BodegaId = inventario.BodegaId;
                item.FechaFinal = inventario.FechaFinal;
                item.Estado = inventario.Estado;
            }
        }

        public IEnumerable<SelectListItem> listaBodegas()
        {
            return dbContext.Bodega.Where(b => b.Estado == true).Select(b => new SelectListItem
            {
                Text = b.Nombre,
                Value = b.Id.ToString()
            });
        }
    }
}
