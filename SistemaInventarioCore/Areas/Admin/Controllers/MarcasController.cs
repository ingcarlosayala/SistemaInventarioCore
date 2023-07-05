using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using SistemaInventarioCore.Models;
using SistemaInventarioCore.Utilidades;
using System.Data;

namespace SistemaInventarioCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class MarcasController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;

        public MarcasController(IUnidadTrabajo unidadTrabajo)
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
        public async Task<IActionResult> Crear(Marca marca)
        {
            if (ModelState.IsValid)
            {
                await unidadTrabajo.Marca.Agregar(marca);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(marca);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var item = await unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());

            if (item is null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Marca marca)
        {
            if (ModelState.IsValid)
            {
                await unidadTrabajo.Marca.Actualizar(marca);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            return View(marca);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todo = await unidadTrabajo.Marca.ObtenerTodos();
            return Json(new {data = todo});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var item = await unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());

            if (item is null)
            {
                return Json(new {success = false, message = "Error al eliminar la marca"});
            }

            unidadTrabajo.Marca.Remover(item);
            await unidadTrabajo.Guardar();
            return Json(new {success = true, message = "Marca eliminada correctamente"});
        }

        #endregion
    }
}
