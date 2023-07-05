using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using SistemaInventarioCore.Models;
using SistemaInventarioCore.Utilidades;

namespace SistemaInventarioCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class BodegasController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public BodegasController(IUnidadTrabajo unidadTrabajo)
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
        public async Task<IActionResult> Crear(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                await unidadTrabajo.Bodega.Agregar(bodega);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(bodega);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var item = await unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());

            if (item is null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                await unidadTrabajo.Bodega.Actualizar(bodega);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(bodega);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todo = await unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new {data = todo});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var item = await unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());

            if (item is null)
            {
                return Json(new {success = false, message = "Error al eliminar la bodega"});
            }

            unidadTrabajo.Bodega.Remover(item);
            await unidadTrabajo.Guardar();
            return Json(new {success = true, message = "Bodega eliminada correctamente"});
        }

        #endregion
    }
}
