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
        public int Año { get; set; }

        public int IdAutor { get; set; }
        public int IdCategoria { get; set; }

        public int Stock { get; set; }

        public string AutorNombre { get; set; }
        public string CategoriaNombre { get; set; }

    }
}

