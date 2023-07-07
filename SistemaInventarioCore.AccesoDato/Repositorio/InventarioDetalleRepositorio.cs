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
    public class InventarioDetalleRepositorio : Repositorio<InventarioDetalle>, IInventarioDetalleRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public InventarioDetalleRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(InventarioDetalle inventarioDetalle)
        {
            var item = await dbContext.InventarioDetalle.FirstOrDefaultAsync(i => i.Id == inventarioDetalle.Id);

            if (item != null)
            {
                item.StockAnterior = inventarioDetalle.StockAnterior;
                item.Cantidad = inventarioDetalle.Cantidad;
            }
        }
    }
}
