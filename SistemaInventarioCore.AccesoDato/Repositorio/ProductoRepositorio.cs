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
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext dbContext;

        public ProductoRepositorio(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Actualizar(Producto producto)
        {
            var item = await dbContext.Producto.FirstOrDefaultAsync(p => p.Id == producto.Id);

            if (item != null)
            {
                item.Codigo = producto.Codigo;
                item.Nombre = producto.Nombre;
                item.Descripcion = producto.Descripcion;
                item.Precio = producto.Precio;
                item.Costo = producto.Costo;
                item.ImagenUrl = producto.ImagenUrl;
                item.CategoriaId = producto.CategoriaId;
                item.MarcaId = producto.MarcaId;
            }
        }
    }
}
