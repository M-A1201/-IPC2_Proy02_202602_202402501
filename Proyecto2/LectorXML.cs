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
                // Aquí procesaremos las listas de categorías y libros en los siguientes pasos
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
}