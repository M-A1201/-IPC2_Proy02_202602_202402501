using System;

class NodoLibro
{
    private Libro libro;
    private NodoLibro siguiente; //puntero al siguiente nodo

    public NodoLibro(Libro libro)
    {
        this.libro = libro;
        this.siguiente = null; 
    }

    // Propiedades o métodos de acceso
    public Libro Libro
    {
        get { return libro; }
    }

    public NodoLibro Siguiente
    {
        get { return siguiente; }
    }

    public NodoLibro GetSiguiente() 
    { 
        return siguiente; 
    }

    // Único método SetSiguiente (borra el duplicado de abajo)
    public void SetSiguiente(NodoLibro siguiente) 
    { 
        this.siguiente = siguiente; 
    }
}