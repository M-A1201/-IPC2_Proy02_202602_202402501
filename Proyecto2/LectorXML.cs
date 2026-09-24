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
}