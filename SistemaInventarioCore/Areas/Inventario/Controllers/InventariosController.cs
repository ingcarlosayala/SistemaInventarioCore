using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioCore.AccesoDato.Data;
using SistemaInventarioCore.AccesoDato.Repositorio;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using SistemaInventarioCore.Models;
using SistemaInventarioCore.Models.ViewsModels;
using SistemaInventarioCore.Utilidades;
using System.Security.Claims;

namespace SistemaInventarioCore.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventario)]
    public class InventariosController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public InventarioVM inventarioVM { get; set; }

        public InventariosController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext dbContext)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NuevoInventario()
        {
            inventarioVM = new InventarioVM()
            {
                Inventario = new Models.Inventario(),
                BodegaLista = unidadTrabajo.Inventario.listaBodegas()
            };

            inventarioVM.Inventario.Estado = false;
            // Obtener el Id del Usuario desde la sesion
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            inventarioVM.Inventario.UsuarioAplicacionId = claim.Value;
            inventarioVM.Inventario.FechaInicial = DateTime.Now;
            inventarioVM.Inventario.FechaFinal = DateTime.Now;

            return View(inventarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NuevoInventario(InventarioVM inventarioVM)
        {
            if (ModelState.IsValid)
            {
                inventarioVM.Inventario.FechaInicial = DateTime.Now;
                inventarioVM.Inventario.FechaFinal = DateTime.Now;
                await unidadTrabajo.Inventario.Agregar(inventarioVM.Inventario);
                await unidadTrabajo.Guardar();
                return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
            }
            inventarioVM.BodegaLista = unidadTrabajo.Inventario.listaBodegas();
            return View(inventarioVM);
        }

        [HttpGet]
        public async Task<IActionResult> DetalleInventario(int id)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.Inventario = await dbContext.Inventario.Include(b => b.Bodega).FirstOrDefaultAsync(d => d.Id == id);
            inventarioVM.InventarioDetalles = dbContext.InventarioDetalle.Include(p => p.Producto.Marca).Where(d => d.InventarioId == id).ToList();
            return View(inventarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetalleInventario(int InventarioId, int productoId, int cantidadId)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.Inventario = await unidadTrabajo.Inventario.ObtenerPrimero(i => i.Id == InventarioId);
            var bodegaProducto = await unidadTrabajo.BodegaProducto.ObtenerPrimero(b => b.ProductoId == productoId && 
                                                                                        b.BodegaId == inventarioVM.Inventario.BodegaId);
            var detalle = await unidadTrabajo.InventarioDetalle.ObtenerPrimero(d => d.InventarioId == InventarioId &&
                                                                                    d.ProductoId == productoId);

            if (detalle == null)
            {
                inventarioVM.InventarioDetalle = new InventarioDetalle();
                inventarioVM.InventarioDetalle.ProductoId = productoId;
                inventarioVM.InventarioDetalle.InventarioId = InventarioId;
                if (bodegaProducto != null)
                {
                    inventarioVM.InventarioDetalle.StockAnterior = bodegaProducto.Cantidad;
                }
                else
                {
                    inventarioVM.InventarioDetalle.StockAnterior = 0;
                }
                inventarioVM.InventarioDetalle.Cantidad = cantidadId;
                await unidadTrabajo.InventarioDetalle.Agregar(inventarioVM.InventarioDetalle);
                await unidadTrabajo.Guardar();
            }
            else
            {
                detalle.Cantidad += cantidadId;
                await unidadTrabajo.Guardar();
            }

            return RedirectToAction("DetalleInventario", new { id = InventarioId });
        }

        [HttpGet]
        public async Task<IActionResult> Mas(int id) //Recibir el id del detalle
        {
            inventarioVM = new InventarioVM();
            var detalle = await unidadTrabajo.InventarioDetalle.Obtener(id);
            inventarioVM.Inventario = await unidadTrabajo.Inventario.Obtener(detalle.InventarioId);

            detalle.Cantidad += 1;
            await unidadTrabajo.Guardar();
            return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Menos(int id) //Recibir el id del detalle
        {
            inventarioVM = new InventarioVM();
            var detalle = await unidadTrabajo.InventarioDetalle.Obtener(id);
            inventarioVM.Inventario = await unidadTrabajo.Inventario.Obtener(detalle.InventarioId);

            if (detalle.Cantidad == 1)
            {
                unidadTrabajo.InventarioDetalle.Remover(detalle);
                await unidadTrabajo.Guardar();
            }
            else
            {
                detalle.Cantidad -= 1;
                await unidadTrabajo.Guardar();
            }
            return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GenerarStock(int id)
        {
            var inventario = await unidadTrabajo.Inventario.Obtener(id);
            var detalleLista = await dbContext.InventarioDetalle.Where(d => d.InventarioId == id).ToListAsync();

            foreach (var item in detalleLista)
            {
                var bodegaProducto = new BodegaProducto();
                bodegaProducto = await unidadTrabajo.BodegaProducto.ObtenerPrimero( b=> b.ProductoId == item.ProductoId &&
                                                                                        b.BodegaId == inventario.BodegaId);

                if (inventario != null) //El registro de stock existe, hay que  actualizar la cantidades
                {
                    bodegaProducto.Cantidad += item.Cantidad;
                    await unidadTrabajo.Guardar();
                }
                else //Registro de stock no existe hay que crearlo
                {
                    bodegaProducto = new BodegaProducto();
                    bodegaProducto.BodegaId = inventario.BodegaId;
                    bodegaProducto.ProductoId = item.ProductoId;
                    bodegaProducto.Cantidad = item.Cantidad;
                    await unidadTrabajo.BodegaProducto.Agregar(bodegaProducto);
                    await unidadTrabajo.Guardar();
                }
            }
            //Actualizar la cabecera del inventario
            inventario.Estado = true;
            inventario.FechaFinal = DateTime.Now;
            await unidadTrabajo.Guardar();
            return RedirectToAction("Index");
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await unidadTrabajo.BodegaProducto.ObtenerTodos(IncluirPropiedad:"Bodega,Producto");
            return Json(new { data = todos });
        }

        [HttpGet]
        public async Task<IActionResult> BuscarProducto(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var listaProducto = await unidadTrabajo.Producto.ObtenerTodos();
                var data = listaProducto.Where(x => x.Codigo.Contains(term,StringComparison.OrdinalIgnoreCase) || 
                                                x.Nombre.Contains(term,StringComparison.OrdinalIgnoreCase)).ToList();

                return Ok(data);
            }

            return Ok();
        }

        #endregion
    }
}
