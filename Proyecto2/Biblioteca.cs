using System;
class Biblioteca
{
    //atributo
    private Categoria categoriaRaiz;
   //constructor
   public Biblioteca()
    {
      this.categoriaRaiz=null;  
    }
    public Categoria GetCategoriaRaiz()
    {
        return categoriaRaiz;
    }
    public void SetCategoriaRaiz(Categoria raiz)
    {
        this.categoriaRaiz=raiz;
    }

}