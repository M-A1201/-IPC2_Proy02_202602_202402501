#nullable disable
using System;

namespace Proyecto2
{
  public class Biblioteca
{
    // Atributo
    private Categoria categoriaRaiz;

    // Constructor
    public Biblioteca()
    {
        this.categoriaRaiz = null;  
    }

    public Categoria GetCategoriaRaiz()
    {
        return categoriaRaiz;
    }

    public void SetCategoriaRaiz(Categoria raiz)
    {
        this.categoriaRaiz = raiz;
    }

    // Método para buscar un libro por su ISBN recorriendo la biblioteca
    public Libro BuscarLibroPorISBN(int isbnBuscado, Categoria categoriaActual)
    {
        if (categoriaActual == null) return null;

        // 1. Buscamos en los libros de la categoría actual
        NodoLibro actualLibro = categoriaActual.GetPrimerLibro();
        while (actualLibro != null)
        {
            if (actualLibro.Libro.ISBN == isbnBuscado)
            {
                return actualLibro.Libro;
            }
            actualLibro = actualLibro.GetSiguiente();
        }

        // 2. Si no está en los libros de esta categoría, buscamos recursivamente en sus subcategorías
        var nodoSub = categoriaActual.GetPrimerSubCategoria();
        while (nodoSub != null)
        {
            Libro encontrado = BuscarLibroPorISBN(isbnBuscado, nodoSub.GetCategoria());
            if (encontrado != null)
            {
                return encontrado;
            }
            nodoSub = nodoSub.GetSiguiente();
        }

        // 3. Si se recorrió todo el árbol y no se encontró, retorna null
        return null;
    }

    // Método para buscar una categoría por su nombre recorriendo recursivamente todo el árbol
    public Categoria BuscarCategoriaPorNombre(string nombreBuscado, Categoria categoriaActual)
    {
        if (categoriaActual == null) return null;

        if (categoriaActual.GetNombre().Equals(nombreBuscado, StringComparison.OrdinalIgnoreCase))
        {
            return categoriaActual;
        }

        var nodoSub = categoriaActual.GetPrimerSubCategoria();
        while (nodoSub != null)
        {
            Categoria encontrada = BuscarCategoriaPorNombre(nombreBuscado, nodoSub.GetCategoria());
            if (encontrada != null)
            {
                return encontrada;
            }
            nodoSub = nodoSub.GetSiguiente();
        }

        return null;
    }

    // Método para encontrar el libro con el ISBN más pequeño del catálogo (recursivo global)
    public Libro ObtenerLibroMenor(Categoria categoriaActual, ref int menorISBN, ref Libro libroMenor)
    {
        if (categoriaActual == null) return libroMenor;

        // 1. Revisamos los libros de la categoría actual
        NodoLibro actualLibro = categoriaActual.GetPrimerLibro();
        while (actualLibro != null)
        {
            if (libroMenor == null || actualLibro.Libro.ISBN < menorISBN)
            {
                menorISBN = actualLibro.Libro.ISBN;
                libroMenor = actualLibro.Libro;
            }
            actualLibro = actualLibro.GetSiguiente();
        }

        // 2. Buscamos en las subcategorías hijas
        var nodoSub = categoriaActual.GetPrimerSubCategoria();
        while (nodoSub != null)
        {
            ObtenerLibroMenor(nodoSub.GetCategoria(), ref menorISBN, ref libroMenor);
            nodoSub = nodoSub.GetSiguiente();
        }

        return libroMenor;
    }

    // Método para encontrar el libro con el ISBN más grande del catálogo (recursivo global)
    public Libro ObtenerLibroMayor(Categoria categoriaActual, ref int mayorISBN, ref Libro libroMayor)
    {
        if (categoriaActual == null) return libroMayor;

        // 1. Revisamos los libros de la categoría actual
        NodoLibro actualLibro = categoriaActual.GetPrimerLibro();
        while (actualLibro != null)
        {
            if (libroMayor == null || actualLibro.Libro.ISBN > mayorISBN)
            {
                mayorISBN = actualLibro.Libro.ISBN;
                libroMayor = actualLibro.Libro;
            }
            actualLibro = actualLibro.GetSiguiente();
        }

        // 2. Buscamos en las subcategorías hijas
        var nodoSub = categoriaActual.GetPrimerSubCategoria();
        while (nodoSub != null)
        {
            ObtenerLibroMayor(nodoSub.GetCategoria(), ref mayorISBN, ref libroMayor);
            nodoSub = nodoSub.GetSiguiente();
        }

        return libroMayor;
    }

    // Método auxiliar recursivo para contar cuántos libros hay en total en todo el árbol
    public int ContarLibros(Categoria categoriaActual)
    {
        if (categoriaActual == null) return 0;

        int contador = 0;
        NodoLibro actual = categoriaActual.GetPrimerLibro();
        while (actual != null)
        {
            contador++;
            actual = actual.GetSiguiente();
        }

        // Sumar recursivamente los libros de las subcategorías hijas
        var nodoSub = categoriaActual.GetPrimerSubCategoria();
        while (nodoSub != null)
        {
            contador += ContarLibros(nodoSub.GetCategoria());
            nodoSub = nodoSub.GetSiguiente();
        }

        return contador;
    }

    // Método auxiliar recursivo para recolectar los libros de todo el árbol en el arreglo
    private void RecopilarLibros(Categoria categoriaActual, Libro[] arreglo, ref int index)
    {
        if (categoriaActual == null) return;

        // 1. Copiamos los libros de la categoría actual
        NodoLibro actual = categoriaActual.GetPrimerLibro();
        while (actual != null)
        {
            arreglo[index] = actual.Libro;
            index++;
            actual = actual.GetSiguiente();
        }

        // 2. Recorremos recursivamente las subcategorías para traer sus libros también
        var nodoSub = categoriaActual.GetPrimerSubCategoria();
        while (nodoSub != null)
        {
            RecopilarLibros(nodoSub.GetCategoria(), arreglo, ref index);
            nodoSub = nodoSub.GetSiguiente();
        }
    }

    // Método para obtener todos los libros del árbol, ordenarlos ascendentemente y mostrarlos
    public void MostrarLibrosOrdenadosAscendente(Categoria categoriaActual)
    {
        int totalLibros = ContarLibros(categoriaActual);
        if (totalLibros == 0)
        {
            Console.WriteLine("\nNo hay libros registrados en el catálogo.");
            return;
        }

        // Creamos el arreglo con el tamaño total real de todo el árbol (los 100 libros)
        Libro[] arregloLibros = new Libro[totalLibros];
        int index = 0;

        // Llenamos el arreglo recorriendo todo el árbol de forma recursiva
        RecopilarLibros(categoriaActual, arregloLibros, ref index);

        // Ordenamos el arreglo usando Bubble Sort por ISBN (Ascendente)
        for (int i = 0; i < totalLibros - 1; i++)
        {
            for (int j = 0; j < totalLibros - i - 1; j++)
            {
                if (arregloLibros[j].ISBN > arregloLibros[j + 1].ISBN)
                {
                    Libro temp = arregloLibros[j];
                    arregloLibros[j] = arregloLibros[j + 1];
                    arregloLibros[j + 1] = temp;
                }
            }
        }

        // Mostramos el resultado ordenado en consola
        Console.WriteLine($"\n=== LIBROS EN ORDEN ASCENDENTE (POR ISBN) - Total: {totalLibros} ===");
        for (int i = 0; i < totalLibros; i++)
        {
            Console.WriteLine($"[ISBN: {arregloLibros[i].ISBN}] - {arregloLibros[i].Titulo} (Autor: {arregloLibros[i].NombreAutor})");
        }
    }

    // Método recursivo para generar el archivo DOT para Graphviz (Categorías, Subcategorías y Libros)
    public void GenerarReporteGraphviz(Categoria categoriaActual, System.IO.StreamWriter writer)
    {
        if (categoriaActual == null) return;

        // Escribimos el nodo de la categoría actual
        string nombreCatLimpio = categoriaActual.GetNombre().Replace(" ", "_");
        writer.WriteLine($"    \"{nombreCatLimpio}\" [label=\"Categoría: {categoriaActual.GetNombre()}\", shape=box, style=filled, fillcolor=lightblue];");

        // Recorremos los libros de esta categoría y los conectamos
        NodoLibro actualLibro = categoriaActual.GetPrimerLibro();
        while (actualLibro != null)
        {
            string idLibro = "Libro_" + actualLibro.Libro.ISBN;
            writer.WriteLine($"    \"{idLibro}\" [label=\"ISBN: {actualLibro.Libro.ISBN}\\n{actualLibro.Libro.Titulo}\\nAutor: {actualLibro.Libro.NombreAutor}\", shape=ellipse, style=filled, fillcolor=lightyellow];");
            writer.WriteLine($"    \"{nombreCatLimpio}\" -> \"{idLibro}\";");
            
            actualLibro = actualLibro.GetSiguiente();
        }

        // Recorremos las subcategorías hijas, las dibujamos y las conectamos con la categoría padre
        var nodoSub = categoriaActual.GetPrimerSubCategoria();
        while (nodoSub != null)
        {
            Categoria subCat = nodoSub.GetCategoria();
            if (subCat != null)
            {
                string nombreSubLimpio = subCat.GetNombre().Replace(" ", "_");
                writer.WriteLine($"    \"{nombreCatLimpio}\" -> \"{nombreSubLimpio}\";"); // Flecha de Padre a Subcategoría
                
                // Llamada recursiva para pintar los elementos de la subcategoría
                GenerarReporteGraphviz(subCat, writer);
            }
            nodoSub = nodoSub.GetSiguiente();
        }
    }

    // Método envoltorio para exportar todo el archivo Graphviz
    public void ExportarGraphviz(string rutaArchivo)
    {
        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(rutaArchivo))
        {
            writer.WriteLine("digraph G {");
            writer.WriteLine("    rankdir=TB;");
            writer.WriteLine("    node [fontname=\"Arial\"];");
            
            GenerarReporteGraphviz(categoriaRaiz, writer);
            
            writer.WriteLine("}");
        }
        Console.WriteLine($"\n¡Reporte Graphviz generado con éxito en '{rutaArchivo}'!");
    }

    // Método para registrar un libro de forma manual
    public bool RegistrarLibroManual(int isbn, string titulo, string autor, string nombreCategoria)
    {
        // Buscamos la categoría destino
        Categoria categoriaDestino = BuscarCategoriaPorNombre(nombreCategoria, categoriaRaiz);
        
        if (categoriaDestino != null)
        {
            Libro nuevoLibro = new Libro(isbn, titulo, autor, nombreCategoria);
            categoriaDestino.AgregarLibro(nuevoLibro);
            return true;
        }
        return false; // Si no encuentra la categoría, no se puede registrar
    }

    // Método recursivo para eliminar un libro por su ISBN en cualquier parte del árbol de categorías
    public bool EliminarLibroPorISBN(int isbnBuscado, Categoria categoriaActual)
    {
        if (categoriaActual == null) return false;

        // 1. Intentamos eliminar el libro en la categoría actual
        NodoLibro actual = categoriaActual.GetPrimerLibro();
        NodoLibro anterior = null;

        while (actual != null)
        {
            if (actual.Libro.ISBN == isbnBuscado)
            {
                if (anterior == null)
                {
                    categoriaActual.SetPrimerLibro(actual.GetSiguiente());
                }
                else
                {
                    anterior.SetSiguiente(actual.GetSiguiente());
                }
                return true; // Encontrado y eliminado con éxito
            }
            anterior = actual;
            actual = actual.GetSiguiente();
        }

        // 2. Si no estaba en esta categoría, buscamos y eliminamos recursivamente en las subcategorías
        var nodoSub = categoriaActual.GetPrimerSubCategoria();
        while (nodoSub != null)
        {
            if (EliminarLibroPorISBN(isbnBuscado, nodoSub.GetCategoria()))
            {
                return true;
            }
            nodoSub = nodoSub.GetSiguiente();
        }

        return false; // No se encontró en todo el árbol
    }

    // Método para mostrar la estructura jerárquica de categorías y sus libros en formato de consola (Estilo Árbol)
public void MostrarEstructuraCompleta(Categoria categoriaActual, string indentacion)
{
    if (categoriaActual == null) return;

    // Mostrar la categoría actual
    Console.WriteLine($"{indentacion}[+] CATEGORIA: {categoriaActual.GetNombre().ToUpper()}");

    // Mostrar los libros que pertenecen directamente a esta categoría
    NodoLibro actualLibro = categoriaActual.GetPrimerLibro();
    while (actualLibro != null)
    {
        if (actualLibro.Libro != null)
        {
            Console.WriteLine($"{indentacion}   |--- ISBN: {actualLibro.Libro.ISBN,-12} | Título: \"{actualLibro.Libro.Titulo,-35}\" | Autor: {actualLibro.Libro.NombreAutor}");
        }
        actualLibro = actualLibro.GetSiguiente();
    }

    // Recorrer recursivamente las subcategorías aumentando la indentación
    // Recorrer recursivamente las subcategorías aumentando la indentación
Categoria.NodoSubCategoria nodoSub = categoriaActual.GetPrimerSubCategoria();
while (nodoSub != null)
{
    if (nodoSub.GetCategoria() != null)
    {
        MostrarEstructuraCompleta(nodoSub.GetCategoria(), indentacion + "    ");
    }
    nodoSub = nodoSub.GetSiguiente();
}
}

// Método para listar únicamente los libros de una categoría específica (Opción 6)
public void MostrarLibrosDeCategoria(Categoria categoria)
{
    if (categoria == null) return;

    NodoLibro actualLibro = categoria.GetPrimerLibro();
    int contador = 0;
    while (actualLibro != null)
    {
        if (actualLibro.Libro != null)
        {
            contador++;
            Console.WriteLine($"[ISBN: {actualLibro.Libro.ISBN}] - {actualLibro.Libro.Titulo} (Autor: {actualLibro.Libro.NombreAutor})");
        }
        actualLibro = actualLibro.GetSiguiente();
    }

    if (contador == 0)
    {
        Console.WriteLine("Esta categoría no contiene libros registrados directamente.");
    }
    else
    {
        Console.WriteLine($"\nTotal de libros en esta categoría: {contador}");
    }
}
}
}