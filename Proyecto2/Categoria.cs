using System;
class Categoria
{
    //atributos de la clase
    private string nombre;
    private Categoria categoriaPadre;

    private NodoSubCategoria primeraSubCategoria;
    //como se le llama a esto?
    private NodoLibro primerLibro;

    //metodo 
    public Categoria(string nombre)
    {
        this.nombre=nombre;
        this.categoriaPadre=null;
        this.primeraSubCategoria=null;
        this.primerLibro=null;

    }

//campos base para la estructura jerarquica
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
        this.primerLibro=nodo;
    }

    class NodoSubCategoria
    {
        private Categoria categoria;
        private NodoSubCategoria siguiente;
        public NodoSubCategoria(Categoria categoria)
        {
            this.categoria=categoria;
            this.siguiente=null;
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
            this.siguiente=siguiente;
        }
    }



}