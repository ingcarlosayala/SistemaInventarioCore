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
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public MarcaRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(Marca marca)
        {
            var item = await dbContext.Marca.FirstOrDefaultAsync(b => b.Id == marca.Id);

            if (item != null)
            {
                item.Nombre = marca.Nombre;
                item.Estado = marca.Estado;
            }
        }

        public IEnumerable<SelectListItem> ListaMarcas()
        {
            return dbContext.Marca.Select(m => new SelectListItem
            {
                Text = m.Nombre,
                Value = m.Id.ToString()
            });
        }
    }
}
