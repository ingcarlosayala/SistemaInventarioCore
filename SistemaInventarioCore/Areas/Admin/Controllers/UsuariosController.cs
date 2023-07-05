using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioCore.AccesoDato.Data;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using SistemaInventarioCore.Utilidades;
using System.Data;

namespace SistemaInventarioCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class UsuariosController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly ApplicationDbContext dbContext;

        public UsuariosController(IUnidadTrabajo unidadTrabajo,ApplicationDbContext dbContext)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioLista = await unidadTrabajo.UsuarioAplicacion.ObtenerTodos();
            var userRole = await dbContext.UserRoles.ToListAsync();
            var roles = await dbContext.Roles.ToListAsync();

            foreach (var usuario in usuarioLista)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }

            return Json(new { data = usuarioLista });
        }

        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuario = await unidadTrabajo.UsuarioAplicacion.ObtenerPrimero(u => u.Id == id);

            if (usuario == null)
            {
                return Json(new {success = false, message = "Error de usuario"});
            }

            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1);
            }

            await unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Operacion Exitosa" });
        }

        #endregion
    }
}
