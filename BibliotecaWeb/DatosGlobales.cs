using Proyecto2;

namespace BibliotecaWeb
{
    public static class DatosGlobales
    {
        public static Biblioteca MiBiblioteca { get; set; } = new Biblioteca();
        public static LectorXML Lector { get; set; } = new LectorXML();
    }
}