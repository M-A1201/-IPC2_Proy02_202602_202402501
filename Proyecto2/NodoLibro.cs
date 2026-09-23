using System;
class NodoLibro
{
    private Libro libro;
    private NodoLibro siguiente; //puntero al siguiente nodo

public NodoLibro(Libro libro)
    {
       this.libro=libro;
       this.siguiente=null; 
    }

//propiedad para los atributos de la clase NodoLibro
    public Libro Libro
    {
        get
        {
            return libro;
        }
       
    }

    public NodoLibro Siguiente
    {
        get
        {
            return siguiente;
        }
    }

    public void SetSiguiente(NodoLibro siguiente)
    {
        this.siguiente=siguiente;
    }

}