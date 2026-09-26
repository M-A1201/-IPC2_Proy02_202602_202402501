#nullable disable
using System;
namespace Proyecto2  
{

public class Program
{
    static void Main(string[] args)
    {
        Biblioteca miBiblioteca = new Biblioteca();
        LectorXML lector = new LectorXML();
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("         CATALOGO DE LIBRERIA           ");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(" 1. Cargar datos desde archivo XML");
            Console.WriteLine(" 2. Registrar nuevo libro en catálogo");
            Console.WriteLine(" 3. Buscar libro por ISBN");
            Console.WriteLine(" 4. Mostrar estructura completa de categorías/libros");
            Console.WriteLine(" 5. Mostrar organización desde una subcategoría");
            Console.WriteLine(" 6. Identificar libros de una categoría determinada");
            Console.WriteLine(" 7. Obtener libro con menor y mayor ISBN");
            Console.WriteLine(" 8. Eliminar libro del catálogo");
            Console.WriteLine(" 9. Generar imágenes de reportes Graphviz (PNG)");
            Console.WriteLine(" 10. Ayuda");
            Console.WriteLine(" 11. Salir");
            Console.Write("\nSeleccione una opción: ");

            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.Write("Ingrese la ruta del archivo XML (ej. entrada.xml): ");
                    string ruta = Console.ReadLine();
                    lector.CargarArchivoConfiguracion(ruta, miBiblioteca);
                    break;

                case "2":
                    Console.Write("Ingrese ISBN: ");
                    if (int.TryParse(Console.ReadLine(), out int nuevoIsbn))
                    {
                        Console.Write("Ingrese Título: ");
                        string nuevoTitulo = Console.ReadLine();
                        Console.Write("Ingrese Autor: ");
                        string nuevoAutor = Console.ReadLine();
                        Console.Write("Ingrese Categoría destino: ");
                        string catDestino = Console.ReadLine();

                        if (miBiblioteca.RegistrarLibroManual(nuevoIsbn, nuevoTitulo, nuevoAutor, catDestino))
                            Console.WriteLine("\n¡Libro registrado exitosamente!");
                        else
                            Console.WriteLine("\nError: No se encontró la categoría especificada.");
                    }
                    else
                    {
                        Console.WriteLine("\nPor favor, ingrese un ISBN válido.");
                    }
                    break;

                case "3":
                    Console.Write("Ingrese el ISBN del libro a buscar: ");
                    if (int.TryParse(Console.ReadLine(), out int isbnBuscado))
                    {
                        Libro encontrado = miBiblioteca.BuscarLibroPorISBN(isbnBuscado, miBiblioteca.GetCategoriaRaiz());
                        if (encontrado != null)
                        {
                            Console.WriteLine($"\n¡Libro encontrado!");
                            Console.WriteLine($"Título: {encontrado.Titulo}");
                            Console.WriteLine($"Autor: {encontrado.NombreAutor}");
                            Console.WriteLine($"ISBN: {encontrado.ISBN}");
                        }
                        else
                        {
                            Console.WriteLine("\nLibro no encontrado en el catálogo.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nPor favor, ingrese un número de ISBN válido.");
                    }
                    break;

                case "4":
                    Console.WriteLine("\n--- ESTRUCTURA COMPLETA DEL CATALOGO ---");
                    miBiblioteca.MostrarEstructuraCompleta(miBiblioteca.GetCategoriaRaiz(), "");
                    break;

                case "5":
                    Console.Write("Ingrese el nombre de la subcategoría desde donde desea ver la organización: ");
                    string subCatNombre = Console.ReadLine();
                    Categoria subEncontrada = miBiblioteca.BuscarCategoriaPorNombre(subCatNombre, miBiblioteca.GetCategoriaRaiz());
                    if (subEncontrada != null)
                    {
                        Console.WriteLine($"\n--- ORGANIZACIÓN DESDE: {subEncontrada.GetNombre().ToUpper()} ---");
                        miBiblioteca.MostrarEstructuraCompleta(subEncontrada, "");
                    }
                    else
                    {
                        Console.WriteLine("\nNo se encontró una categoría con ese nombre.");
                    }
                    break;

                case "6":
                    Console.Write("Ingrese el nombre de la categoría para listar sus libros: ");
                    string catFiltro = Console.ReadLine();
                    Categoria catLibros = miBiblioteca.BuscarCategoriaPorNombre(catFiltro, miBiblioteca.GetCategoriaRaiz());
                    if (catLibros != null)
                    {
                        Console.WriteLine($"\n--- LIBROS EN LA CATEGORÍA: {catLibros.GetNombre().ToUpper()} ---");
                        miBiblioteca.MostrarLibrosDeCategoria(catLibros);
                    }
                    else
                    {
                        Console.WriteLine("\nNo se encontró la categoría especificada.");
                    }
                    break;

                case "7":
                    int menor = int.MaxValue;
                    int mayor = int.MinValue;
                    Libro libMenor = null;
                    Libro libMayor = null;

                    libMenor = miBiblioteca.ObtenerLibroMenor(miBiblioteca.GetCategoriaRaiz(), ref menor, ref libMenor);
                    libMayor = miBiblioteca.ObtenerLibroMayor(miBiblioteca.GetCategoriaRaiz(), ref mayor, ref libMayor);

                    Console.WriteLine("\n=== ESTADISTICAS DE ISBN ===");
                    if (libMenor != null)
                        Console.WriteLine($"Libro con ISBN Menor: [{libMenor.ISBN}] {libMenor.Titulo}");
                    else
                        Console.WriteLine("No hay libros registrados.");

                    if (libMayor != null)
                        Console.WriteLine($"Libro con ISBN Mayor: [{libMayor.ISBN}] {libMayor.Titulo}");
                    else
                        Console.WriteLine("No hay libros registrados.");
                    break;

                case "8":
                    Console.Write("Ingrese el ISBN del libro que desea eliminar: ");
                    if (int.TryParse(Console.ReadLine(), out int isbnEliminar))
                    {
                        bool eliminado = miBiblioteca.EliminarLibroPorISBN(isbnEliminar, miBiblioteca.GetCategoriaRaiz());
                        if (eliminado)
                            Console.WriteLine("\n¡Libro eliminado exitosamente del catálogo!");
                        else
                            Console.WriteLine("\nError: No se encontró ningún libro con ese ISBN en el sistema.");
                    }
                    else
                    {
                        Console.WriteLine("\nPor favor, ingrese un número de ISBN válido.");
                    }
                    break;

                case "9":
                    miBiblioteca.ExportarGraphviz("reporte_biblioteca.dot");
                    try
                    {
                        var startInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "dot",
                            Arguments = "-Tpng reporte_biblioteca.dot -o reporte.png",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        using (var process = System.Diagnostics.Process.Start(startInfo))
                        {
                            process.WaitForExit();
                        }
                        Console.WriteLine("¡Imagen 'reporte.png' generada automáticamente con éxito!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nNo se pudo generar la imagen automáticamente. Asegúrate de tener Graphviz instalado.");
                        Console.WriteLine($"Detalle: {ex.Message}");
                    }
                    break;

               // Agrega esta opción dentro de tu estructura de menú (Switch o If-Else)
case "10": // O el valor que utilices para la ayuda
    Console.Clear();
    Console.WriteLine("==================================================");
    Console.WriteLine("                AYUDA / INFORMACIÓN               ");
    Console.WriteLine("==================================================");
    Console.WriteLine(" Curso: Introducción a la Programación y Computación 2");
    Console.WriteLine(" Estudiante: Manuel Angel Tíu Sanic");
    Console.WriteLine(" Carnet: [Tu Número de Carnet]");
    Console.WriteLine("--------------------------------------------------");
    Console.WriteLine(" Enlace a la Documentación del Proyecto:");
    Console.WriteLine(" https://github.com/tu-usuario/IPC2_Proy02_202602_#Carnet");
    Console.WriteLine("==================================================");
    Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
    Console.ReadKey();
    break;
                    case "11":
                    salir = true;
                    Console.WriteLine("\nSaliendo del sistema... ¡Mucho éxito con tu proyecto!");
                    break;

                default:
                    Console.WriteLine("\nOpción no válida. Intente de nuevo.");
                    break;
            }
        }
    }
}
}