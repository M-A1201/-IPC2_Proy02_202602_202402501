using Microsoft.AspNetCore.Mvc;
using Proyecto2;

namespace BibliotecaWeb.Controllers
{
    public class BibliotecaController : Controller
    {
        // ========== OPCIÓN 2: Registrar Libro ==========
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(string isbn, string titulo, string autor, string categoria)
        {
            try
            {
                // Verificamos que ya se haya cargado algo
                if (DatosGlobales.MiBiblioteca.GetCategoriaRaiz() == null)
                {
                    TempData["Mensaje"] = "⚠️ Primero debe cargar el archivo XML (Opción 1).";
                    TempData["TipoAlerta"] = "warning";
                    return RedirectToAction("Registrar");
                }

                if (int.TryParse(isbn, out int isbnNum))
                {
                    bool exito = DatosGlobales.MiBiblioteca.RegistrarLibroManual(
                        isbnNum, titulo, autor, categoria
                    );
                    
                    if (exito)
                    {
                        TempData["Mensaje"] = $"✅ Libro registrado: {titulo} (ISBN: {isbn})";
                        TempData["TipoAlerta"] = "success";
                    }
                    else
                    {
                        TempData["Mensaje"] = $"❌ No existe la categoría '{categoria}'.";
                        TempData["TipoAlerta"] = "danger";
                    }
                }
                else
                {
                    TempData["Mensaje"] = "❌ ISBN debe ser un número.";
                    TempData["TipoAlerta"] = "danger";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error: " + ex.Message;
                TempData["TipoAlerta"] = "danger";
            }
            return RedirectToAction("Registrar");
        }

        // ========== OPCIÓN 3: Buscar Libro por ISBN ==========
        [HttpGet]
        public IActionResult Buscar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Buscar(string isbn)
        {
            try
            {
                // MISMA instancia donde cargaste el XML ✅
                if (DatosGlobales.MiBiblioteca.GetCategoriaRaiz() == null)
                {
                    TempData["Mensaje"] = "⚠️ Primero cargue el archivo XML.";
                    TempData["TipoAlerta"] = "warning";
                    return View();
                }

                if (int.TryParse(isbn, out int isbnBuscado))
                {
                    Libro encontrado = DatosGlobales.MiBiblioteca.BuscarLibroPorISBN(
                        isbnBuscado,
                        DatosGlobales.MiBiblioteca.GetCategoriaRaiz()
                    );
                    
                    if (encontrado != null)
                    {
                        ViewBag.Libro = encontrado;
                        TempData["Mensaje"] = "✅ Libro encontrado";
                        TempData["TipoAlerta"] = "success";
                    }
                    else
                    {
                        TempData["Mensaje"] = "❌ No existe ningún libro con ese ISBN.";
                        TempData["TipoAlerta"] = "danger";
                    }
                }
                else
                {
                    TempData["Mensaje"] = "❌ Ingrese un número válido.";
                    TempData["TipoAlerta"] = "danger";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error: " + ex.Message;
                TempData["TipoAlerta"] = "danger";
            }
            return View();
        }

        // ========== Otras opciones (las dejamos listas) ==========
        [HttpGet] public IActionResult Subcategoria() => View();
        [HttpGet] public IActionResult Identificar() => View();
        [HttpGet] public IActionResult ExtremosIsbn() => View();
        [HttpGet] public IActionResult Eliminar() => View();
        [HttpGet] public IActionResult Reportes() => View();
    }
}