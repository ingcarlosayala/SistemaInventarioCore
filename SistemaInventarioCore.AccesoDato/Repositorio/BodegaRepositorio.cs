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
    public class BodegaRepositorio : Repositorio<Bodega>, IBodegaRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public BodegaRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(Bodega bodega)
        {
            var item = await dbContext.Bodega.FirstOrDefaultAsync(b => b.Id == bodega.Id);

            if (item != null)
            {
                item.Nombre = bodega.Nombre;
                item.Descripcion = bodega.Descripcion;
                item.Estado = bodega.Estado;
            }
        }
    }
}
