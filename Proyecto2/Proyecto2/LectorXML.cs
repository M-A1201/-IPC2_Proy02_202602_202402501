#nullable disable
using System;
using System.Collections.Generic;
using System.Xml;

namespace Proyecto2
{
    public class LectorXML
    {
        public void CargarArchivoConfiguracion(string rutaArchivo, Biblioteca biblioteca)
        {
            try
            {
                XmlDocument documento = new XmlDocument();
                documento.Load(rutaArchivo);
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
                Console.WriteLine("Error al leer el archivo XML: " + ex.Message);
            }
        }

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
                int intentosMaximos = 50;
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
                            Categoria categoriaPadre = biblioteca.BuscarCategoriaPorNombre(nombrePadre, biblioteca.GetCategoriaRaiz());
                            if (categoriaPadre != null)
                            {
                                categoriaPadre.InsertarSubCategoriaOrdenada(nuevaCategoria);
                                Console.WriteLine($"Subcategoría procesada: {nombreCategoria}, Padre: {nombrePadre}");
                            }
                            else
                            {
                                noInsertados.Add(nodoCat);
                            }
                        }
                    }
                    if (noInsertados.Count == pendientes.Count)
                    {
                        break;
                    }
                    pendientes = noInsertados;
                }
            }
        }

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
                    
                    Libro nuevoLibro = new Libro(isbn, titulo, autor, nombreCategoria);
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
}