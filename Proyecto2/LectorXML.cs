using System;
using System.Xml;
class LectorXML
{
    // Método principal que recibe la ruta del archivo XML y la biblioteca a poblar
    public void CargarArchivoConfiguracion(string rutaArchivo, Biblioteca biblioteca)
    {
        //tenemos un try catch pero no entiendo lo que esta en el catch
        try
        {
            XmlDocument documento = new XmlDocument();
            documento.Load(rutaArchivo);//no se que es

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
            Console.WriteLine("Error al leer el archivo XML: " + ex.Message);
        }
    }

    // Método para procesar las categorías del XML
    public void ProcesarCategorias(XmlNode nodoConfig, Biblioteca biblioteca)
    {
        // Buscamos la etiqueta <listaCategorias> dentro de la configuración
        XmlNode listaCatNode = nodoConfig.SelectSingleNode("listaCategorias");//que es listaCat? por que Cat?

        if (listaCatNode != null)
        {
            // Recorremos cada etiqueta <categoria> que se encuentre en la lista
            foreach (XmlNode nodoCat in listaCatNode.SelectNodes("categoria"))
            {
                string nombreCategoria = nodoCat.InnerText.Trim();
                
                // Verificamos si tiene el atributo padre (es opcional)
                string nombrePadre = "";
                if (nodoCat.Attributes["padre"] != null)
                {
                    nombrePadre = nodoCat.Attributes["padre"].Value.Trim();
                }

                // Aquí posteriormente enlazaremos la categoría al árbol general de la biblioteca
                Console.WriteLine($"Categoría leída: {nombreCategoria}, Padre: {(string.IsNullOrEmpty(nombrePadre) ? "Ninguno (Raíz)" : nombrePadre)}");
            }
        }
    }

    // Método para procesar los libros del XML
    public void ProcesarLibros(XmlNode nodoConfig, Biblioteca biblioteca)
    {
        // Buscamos la etiqueta <listaLibros> dentro de la configuración
        XmlNode listaLibrosNode = nodoConfig.SelectSingleNode("listaLibros");

        if (listaLibrosNode != null)
        {
            // Recorremos cada etiqueta <libro> dentro de la lista
            foreach (XmlNode nodoLibro in listaLibrosNode.SelectNodes("libro"))
            {
                int isbn = int.Parse(nodoLibro.SelectSingleNode("ISBN").InnerText.Trim());
                string titulo = nodoLibro.SelectSingleNode("titulo").InnerText.Trim();
                string autor = nodoLibro.SelectSingleNode("autor").InnerText.Trim();
                string categoriaLibro = nodoLibro.SelectSingleNode("categoria").InnerText.Trim();

                // Aquí posteriormente asviaremos este libro a su respectiva categoría
                Console.WriteLine($"Libro leído: [ISBN: {isbn}] {titulo} por {autor}");
            }
        }
    }
}