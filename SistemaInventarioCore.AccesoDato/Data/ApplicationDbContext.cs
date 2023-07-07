using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioCore.Models;

namespace SistemaInventarioCore.AccesoDato.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Models
        public DbSet<Bodega> Bodega { get; set; }
        public DbSet<Marca> Marca { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }
        public DbSet<BodegaProducto> BodegaProducto { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<InventarioDetalle> InventarioDetalle { get; set; }
        public DbSet<KardexInventario> KardexInventario { get; set; }
    }
}