using Microsoft.AspNetCore.Mvc;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using SistemaInventarioCore.Models;

namespace SistemaInventarioCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public CategoriasController(IUnidadTrabajo unidadTrabajo)
        {
            this.unidadTrabajo = unidadTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Categoria categoria)
        {

            if (ModelState.IsValid)
            {
                if (await unidadTrabajo.Existe(categoria.Nombre))
                {
                    return RedirectToAction(nameof(Index));
                }

                await unidadTrabajo.Categoria.Agregar(categoria);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var item = await unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());

            if (item is null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await unidadTrabajo.Categoria.Actualizar(categoria);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todo = await unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new {data = todo});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var item = await unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());

            if (item is null)
            {
                return Json(new {success = false, message = "Error al eliminar la marca"});
            }

            unidadTrabajo.Categoria.Remover(item);
            await unidadTrabajo.Guardar();
            return Json(new {success = true, message = "Categoria eliminada correctamente"});
        }

        #endregion
    }
}
