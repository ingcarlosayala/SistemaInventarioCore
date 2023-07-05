using Microsoft.EntityFrameworkCore;
using SistemaInventarioCore.AccesoDato.Data;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.AccesoDato.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext dbContext;

        public IBodegaRepositorio Bodega { get; private set; }
        public IMarcaRepositorio Marca { get; private set; }
        public ICategoriaRepositorio Categoria { get; private set; }
        public IProductoRepositorio Producto { get; private set; }
        public IUsuarioAplicacionRepositorio UsuarioAplicacion { get; private set; }

        public UnidadTrabajo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            Bodega = new BodegaRepositorio(dbContext);
            Marca = new MarcaRepositorio(dbContext);
            Categoria = new CategoriaRepositorio(dbContext);
            Producto = new ProductoRepositorio(dbContext);
            UsuarioAplicacion = new UsuarioAplicacionRepositorio(dbContext);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public async Task Guardar()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Existe(string nombre)
        {
            return await dbContext.Categoria.AnyAsync(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        }
    }
}
