#nullable disable
using System;
using System.Collections.Generic;
using System.Xml;

class LectorXML
{
    // Método principal que recibe la ruta del archivo XML y la biblioteca a poblar
    public void CargarArchivoConfiguracion(string rutaArchivo, Biblioteca biblioteca)
    {
        try
        {
            XmlDocument documento = new XmlDocument();
            documento.Load(rutaArchivo); // Carga el archivo físico a la memoria como un árbol XML

            // Obtenemos el nodo raíz <config> del archivo XML
            XmlNode nodoConfig = documento.SelectSingleNode("//config");

            if (nodoConfig != null)
            {
                Console.WriteLine("Archivo XML cargado correctamente.");
                ProcesarCategorias(nodoConfig, biblioteca);
                ProcesarLibros(nodoConfig, biblioteca);
            }
        }
        catch (Exception ex)
        {
            // Bloque salvavidas: captura cualquier error de ruta o sintaxis sin cerrar la consola
            Console.WriteLine("Error al leer el archivo XML: " + ex.Message);
        }
    }

    // Método mejorado con reintentos para procesar categorías sin importar el orden en el XML
    public void ProcesarCategorias(XmlNode nodoConfig, Biblioteca biblioteca)
    {
        XmlNode listaCatNode = nodoConfig.SelectSingleNode("listaCategorias");

        if (listaCatNode != null)
        {
            XmlNodeList nodosCategoria = listaCatNode.SelectNodes("categoria");
            List<XmlNode> pendientes = new List<XmlNode>();

            foreach (XmlNode nodo in nodosCategoria)
            {
                pendientes.Add(nodo);
            }

            int intentosMaximos = 50; // Seguridad para evitar bucles infinitos
            while (pendientes.Count > 0 && intentosMaximos > 0)
            {
                intentosMaximos--;
                List<XmlNode> noInsertados = new List<XmlNode>();

                foreach (XmlNode nodoCat in pendientes)
                {
                    string nombreCategoria = nodoCat.InnerText.Trim();
                    string nombrePadre = "";
                    if (nodoCat.Attributes["padre"] != null)
                    {
                        nombrePadre = nodoCat.Attributes["padre"].Value.Trim();
                    }

                    Categoria nuevaCategoria = new Categoria(nombreCategoria);

                    // Si no tiene padre o es la raíz principal
                    if (string.IsNullOrEmpty(nombrePadre) || nombrePadre.ToLower() == "catalogo" || nombrePadre.ToLower() == "ninguno")
                    {
                        if (biblioteca.GetCategoriaRaiz() == null)
                        {
                            biblioteca.SetCategoriaRaiz(nuevaCategoria);
                            Console.WriteLine($"Categoría Raíz procesada: {nombreCategoria}");
                        }
                    }
                    else
                    {
                        // Intentamos buscar el padre en el árbol actual
                        Categoria categoriaPadre = biblioteca.BuscarCategoriaPorNombre(nombrePadre, biblioteca.GetCategoriaRaiz());
                        if (categoriaPadre != null)
                        {
                            categoriaPadre.InsertarSubCategoriaOrdenada(nuevaCategoria);
                            Console.WriteLine($"Subcategoría procesada: {nombreCategoria}, Padre: {nombrePadre}");
                        }
                        else
                        {
                            // Si el padre todavía no existe, lo dejamos para el siguiente ciclo
                            noInsertados.Add(nodoCat);
                        }
                    }
                }

                // Si en una vuelta completa no se pudo insertar ninguno, rompemos para evitar bucle
                if (noInsertados.Count == pendientes.Count)
                {
                    break;
                }
                pendientes = noInsertados;
            }
        }
    }

    // Método para procesar los libros del XML y enlazarlos por su categoría
    public void ProcesarLibros(XmlNode nodoConfig, Biblioteca biblioteca)
    {
        XmlNode listaLibrosNode = nodoConfig.SelectSingleNode("listaLibros");

        if (listaLibrosNode != null)
        {
            foreach (XmlNode nodoLibro in listaLibrosNode.SelectNodes("libro"))
            {
                int isbn = int.Parse(nodoLibro.SelectSingleNode("ISBN").InnerText.Trim());
                string titulo = nodoLibro.SelectSingleNode("titulo").InnerText.Trim();
                string autor = nodoLibro.SelectSingleNode("autor").InnerText.Trim();
                string nombreCategoria = nodoLibro.SelectSingleNode("categoria").InnerText.Trim();

                // 1. Creamos la instancia del objeto Libro
                Libro nuevoLibro = new Libro(isbn, titulo, autor, nombreCategoria);

                // 2. Buscamos la categoría en la biblioteca para agregarle el libro
                Categoria categoriaDestino = biblioteca.BuscarCategoriaPorNombre(nombreCategoria, biblioteca.GetCategoriaRaiz());
                
                if (categoriaDestino != null)
                {
                    categoriaDestino.AgregarLibro(nuevoLibro);
                    Console.WriteLine($"Libro leído y guardado en [{nombreCategoria}]: [ISBN: {isbn}] {titulo}");
                }
                else
                {
                    Console.WriteLine($"Advertencia: No se encontró la categoría '{nombreCategoria}' para el libro {titulo}");
                }
            }
        }
    }
}