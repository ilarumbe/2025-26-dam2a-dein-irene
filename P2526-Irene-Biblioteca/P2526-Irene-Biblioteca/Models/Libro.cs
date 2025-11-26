using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2526_Irene_Biblioteca.Models
{
    public class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public int Ano { get; set; }

        public int IdAutor { get; set; }
        public int IdCategoria { get; set; }

        public int Stock { get; set; }

        public Autor Autor { get; set; }
        public Categoria Categoria { get; set; }
    }
}
