#nullable disable
using System;

class Categoria
{
    // Atributos de la clase
    private string nombre;
    private Categoria categoriaPadre;

    private NodoSubCategoria primeraSubCategoria;
    private NodoLibro primerLibro;

    // Constructor
    public Categoria(string nombre)
    {
        this.nombre = nombre;
        this.categoriaPadre = null;
        this.primeraSubCategoria = null;
        this.primerLibro = null;
    }

    // Getters y Setters base
    public string GetNombre()
    {
        return nombre;
    }

    public NodoLibro GetPrimerLibro()
    {
        return primerLibro;
    }

    public void SetPrimerLibro(NodoLibro nodo)
    {
        this.primerLibro = nodo;
    }

    // Getter necesario para obtener la primera subcategoría desde la Biblioteca
    public NodoSubCategoria GetPrimerSubCategoria()
    {
        return primeraSubCategoria;
    }

    // Clase NodoSubCategoria hecha pública para acceso externo seguro
    public class NodoSubCategoria
    {
        private Categoria categoria;
        private NodoSubCategoria siguiente;

        public NodoSubCategoria(Categoria categoria)
        {
            this.categoria = categoria;
            this.siguiente = null;
        }

        public Categoria GetCategoria()
        {
            return categoria;
        }

        public NodoSubCategoria GetSiguiente()
        {
            return siguiente;
        }

        public void SetSiguiente(NodoSubCategoria siguiente)
        {
            this.siguiente = siguiente;
        }
    }

    // Método para agregar una subcategoría manteniendo el orden alfabético
    public void InsertarSubCategoriaOrdenada(Categoria nuevaCategoria)
    {
        NodoSubCategoria nuevoNodo = new NodoSubCategoria(nuevaCategoria);

        if (primeraSubCategoria == null || string.Compare(nuevaCategoria.GetNombre(), primeraSubCategoria.GetCategoria().GetNombre(), StringComparison.OrdinalIgnoreCase) < 0)
        {
            nuevoNodo.SetSiguiente(primeraSubCategoria);
            primeraSubCategoria = nuevoNodo;
            return;
        }

        NodoSubCategoria actual = primeraSubCategoria;
        while (actual.GetSiguiente() != null && string.Compare(actual.GetSiguiente().GetCategoria().GetNombre(), nuevaCategoria.GetNombre(), StringComparison.OrdinalIgnoreCase) < 0)
        {
            actual = actual.GetSiguiente();
        }

        nuevoNodo.SetSiguiente(actual.GetSiguiente());
        actual.SetSiguiente(nuevoNodo);
    }

    // Método para agregar un libro a la lista enlazada de esta categoría
    public void AgregarLibro(Libro nuevoLibro)
    {
        NodoLibro nuevoNodo = new NodoLibro(nuevoLibro);

        if (primerLibro == null)
        {
            primerLibro = nuevoNodo;
        }
        else
        {
            NodoLibro actual = primerLibro;
            while (actual.GetSiguiente() != null)
            {
                actual = actual.GetSiguiente();
            }
            actual.SetSiguiente(nuevoNodo);
        }
    }

    // Método para eliminar un libro de su lista enlazada propia
    public bool EliminarLibro(int isbnBuscado)
    {
        NodoLibro actual = primerLibro;
        NodoLibro anterior = null;

        while (actual != null)
        {
            if (actual.Libro.ISBN == isbnBuscado)
            {
                if (anterior == null)
                {
                    primerLibro = actual.GetSiguiente();
                }
                else
                {
                    anterior.SetSiguiente(actual.GetSiguiente());
                }
                return true;
            }
            anterior = actual;
            actual = actual.GetSiguiente();
        }
        return false;
    }
}