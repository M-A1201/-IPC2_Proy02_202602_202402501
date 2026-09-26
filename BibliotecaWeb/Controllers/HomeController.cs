using Microsoft.AspNetCore.Mvc;
using Proyecto2;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace BibliotecaWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Ayuda()
        {
            return View();
        }

        // Página del formulario de carga
        [HttpGet]
        public IActionResult CargarXml()
        {
            return View();
        }

        // Procesa el archivo enviado
        [HttpPost]
        public IActionResult CargarXml(IFormFile archivoXml)
        {
            if (archivoXml != null && archivoXml.Length > 0)
            {
                try
                {
                    var rutaTemporal = Path.Combine(Path.GetTempPath(), archivoXml.FileName);
                    using (var stream = new FileStream(rutaTemporal, FileMode.Create))
                    {
                        archivoXml.CopyTo(stream);
                    }

                    // Reiniciamos y cargamos en la instancia compartida
                    DatosGlobales.MiBiblioteca = new Biblioteca();
                    DatosGlobales.Lector.CargarArchivoConfiguracion(rutaTemporal, DatosGlobales.MiBiblioteca);

                    if (DatosGlobales.MiBiblioteca.GetCategoriaRaiz() != null)
                    {
                        TempData["Mensaje"] = "Archivo XML cargado y procesado con éxito!";
                        TempData["TipoAlerta"] = "success";
                    }
                    else
                    {
                        TempData["Mensaje"] = " El archivo se leyó, pero no se encontró una categoría raíz válida.";
                        TempData["TipoAlerta"] = "warning";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = "Error al procesar: " + ex.Message;
                    TempData["TipoAlerta"] = "danger";
                }
            }
            else
            {
                TempData["Mensaje"] = "Por favor selecciona un archivo XML válido.";
                TempData["TipoAlerta"] = "warning";
            }
            return RedirectToAction("CargarXml");
        }

        public IActionResult Estructura()
        {
            var raiz = DatosGlobales.MiBiblioteca.GetCategoriaRaiz();
            return View(raiz);
        }
    }
}