using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2526_Irene_Biblioteca.Models
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }

        public int IdLibro { get; set; }
        public int IdCliente { get; set; }
        public int IdEmpleado { get; set; }

        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public bool Devuelto { get; set; }

        public Libro Libro { get; set; }
        public Cliente Cliente { get; set; }
        public Empleado Empleado { get; set; }
    }
}

