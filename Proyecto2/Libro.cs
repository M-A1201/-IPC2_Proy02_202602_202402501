using System;
class Libro
{
    //atributos
    private string titulo;
    private int isbn;
    private string nombreAutor;
    private string categoria;


    //constructor
    public Libro(int isbn, string titulo, string nombreAutor, string categoria)
    {
        this.isbn=isbn;
        this.titulo=titulo;
        this.nombreAutor=nombreAutor;
        this.categoria=categoria;  
    }

    //propiedades(metodos getter y setter) para acceder al los atributos
    public string Titulo
    {
        get
    {
        return titulo;
    }
       
    }
    public string NombreAutor
    {
        get
    {
        return nombreAutor;
    }
        
    }

    public string Categoria
    {
        get
    {
        return categoria;
    }
    
    }

    public int ISBN
    {
        get
    {
        return isbn;
    }
       
    }

}