using Microsoft.AspNetCore.Mvc;
using SistemaInventarioCore.AccesoDato.Repositorio.IRepositorio;
using SistemaInventarioCore.Models;
using SistemaInventarioCore.Models.ViewsModels;
using System.Drawing;

namespace SistemaInventarioCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductosController : Controller
    {
        private readonly IUnidadTrabajo unidadTrabajo;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public ProductoVM productoVM { get; set; }

        public ProductosController(IUnidadTrabajo unidadTrabajo,IWebHostEnvironment webHostEnvironment)
        {
            this.unidadTrabajo = unidadTrabajo;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                ListaCategorias =  unidadTrabajo.Categoria.ListaCategorias(),
                ListaMarcas =  unidadTrabajo.Marca.ListaMarcas(),
            };

            return View(productoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (productoVM.Producto.Id == 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string Upload = Path.Combine(webRootPath, @"imagenes\producto");
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStrems = new FileStream(Path.Combine(Upload,fileName + extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStrems);
                    }

                    productoVM.Producto.ImagenUrl = @"\imagenes\producto\" + fileName + extension;

                    await unidadTrabajo.Producto.Agregar(productoVM.Producto);
                    await unidadTrabajo.Guardar();
                    return RedirectToAction(nameof(Index));
                }
            }

            productoVM.ListaCategorias = unidadTrabajo.Categoria.ListaCategorias();
            productoVM.ListaMarcas = unidadTrabajo.Marca.ListaMarcas();
            
            return View(productoVM);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                ListaCategorias = unidadTrabajo.Categoria.ListaCategorias(),
                ListaMarcas = unidadTrabajo.Marca.ListaMarcas()
            };

            if (id == null)
            {
                return NotFound();
            }

            productoVM.Producto = await unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());

            if (productoVM.Producto == null)
            {
                return NotFound();
            }

            return View(productoVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var imagenDB = await unidadTrabajo.Producto.Obtener(productoVM.Producto.Id);

                if (files.Count() > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    string upload = Path.Combine(webRootPath, @"imagenes\producto");
                    var extension = Path.GetExtension(files[0].FileName);

                    var imagenRuta = Path.Combine(webRootPath, imagenDB.ImagenUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(imagenRuta)) //renplazar imagen
                    {
                        System.IO.File.Delete(imagenRuta);
                    }

                    using (var fileStrems = new FileStream(Path.Combine(upload,fileName + extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStrems);
                    }

                    productoVM.Producto.ImagenUrl = @"\imagenes\producto\" + fileName + extension;

                    await unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                    await unidadTrabajo.Guardar();
                    return RedirectToAction(nameof(Index));
                }
                else //No renplazar la imagen
                {
                    productoVM.Producto.ImagenUrl = imagenDB.ImagenUrl;
                }

                await unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                await unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }

            productoVM.ListaCategorias = unidadTrabajo.Categoria.ListaCategorias();
            productoVM.ListaMarcas = unidadTrabajo.Marca.ListaMarcas();

            return View(productoVM);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todo = await unidadTrabajo.Producto.ObtenerTodos(IncluirPropiedad:"Categoria,Marca");
            return Json(new { data = todo });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            string webRootPath = webHostEnvironment.WebRootPath;

            var producto = await unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());

            if (producto == null)
            {
                return Json(new { success = false, message = "Error al eliminar el producto" });
            }

            var imagenRuta = Path.Combine(webRootPath,producto.ImagenUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagenRuta))
            {
                System.IO.File.Delete(imagenRuta);
            }

            unidadTrabajo.Producto.Remover(producto);
            await unidadTrabajo.Guardar();
            return Json(new {success = true, message = "Producto eliminado correctamente"});
        }

        #endregion
    }
}
