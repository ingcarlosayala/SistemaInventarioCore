using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo:IDisposable
    {
        IBodegaRepositorio Bodega { get; }
        IMarcaRepositorio Marca { get; }
        ICategoriaRepositorio Categoria { get; }
        IProductoRepositorio Producto { get; }
        IUsuarioAplicacionRepositorio UsuarioAplicacion { get; }

        Task<bool> Existe(string nombre);

        Task Guardar();
    }
}
