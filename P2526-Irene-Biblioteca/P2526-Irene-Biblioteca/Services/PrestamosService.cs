using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Repositories;
using System;

namespace P2526_Irene_Biblioteca.Services
{
    public class PrestamosService
    {
        private readonly LibrosRepository repoLibros = new LibrosRepository();

        public bool PuedeEditar() => SesionActual.EsEmpleado;

        public bool ValidarAlta(object libroSel, object clienteSel, object empleadoSel, DateTime fechaPrestamo, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (libroSel == null || clienteSel == null || empleadoSel == null)
            {
                error = "Selecciona Libro, Cliente y Empleado.";
                return false;
            }

            if (fechaPrestamo == default)
            {
                error = "Fecha préstamo inválida.";
                return false;
            }

            error = null;
            return true;
        }

        public bool PuedePrestar(int idLibro, out string error)
        {
            int stock = repoLibros.GetStock(idLibro);
            if (stock <= 0)
            {
                error = "No hay stock disponible para este libro.";
                return false;
            }

            error = null;
            return true;
        }

        public void AplicarPrestamoStock(int idLibro)
        {
            int stock = repoLibros.GetStock(idLibro);
            repoLibros.UpdateStock(idLibro, stock - 1);
        }

        public void AplicarDevolucionStock(int idLibro)
        {
            int stock = repoLibros.GetStock(idLibro);
            repoLibros.UpdateStock(idLibro, stock + 1);
        }
    }
}
