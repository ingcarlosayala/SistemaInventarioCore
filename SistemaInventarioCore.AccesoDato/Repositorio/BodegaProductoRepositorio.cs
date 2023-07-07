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
    public class BodegaProductoRepositorio : Repositorio<BodegaProducto>, IBodegaProductoRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public BodegaProductoRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(BodegaProducto bodegaProducto)
        {
            var item = await dbContext.BodegaProducto.FirstOrDefaultAsync(bp => bp.Id == bodegaProducto.Id);

            if (item != null)
            {
                item.Cantidad = bodegaProducto.Cantidad;
            }
        }
    }
}
