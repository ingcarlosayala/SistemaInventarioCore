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
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriaRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(Categoria categoria)
        {
            var item = await dbContext.Categoria.FirstOrDefaultAsync(b => b.Id == categoria.Id);

            if (item != null)
            {
                item.Nombre = categoria.Nombre;
                item.Estado = categoria.Estado;
            }
        }

        public IEnumerable<SelectListItem> ListaCategorias()
        {
            return dbContext.Categoria.Select(m => new SelectListItem
            {
                Text = m.Nombre,
                Value = m.Id.ToString()
            });
        }
    }
}
