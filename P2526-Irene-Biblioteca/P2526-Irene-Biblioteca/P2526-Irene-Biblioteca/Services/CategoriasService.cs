using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Repositories;

namespace P2526_Irene_Biblioteca.Services
{
    public class CategoriasService
    {
        private readonly LibrosRepository repoLibros = new LibrosRepository();
        public bool PuedeEditar()
        {
            return SesionActual.EsEmpleado;
        }

        public bool ValidarAlta(string nombre, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                error = "El nombre de la categoría es obligatorio.";
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidarModificacion(int? idCategoria, string nombre, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idCategoria == null || idCategoria <= 0)
            {
                error = "Selecciona una categoría.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                error = "El nombre de la categoría es obligatorio.";
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidarBorrado(int? idCategoria, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idCategoria == null || idCategoria <= 0)
            {
                error = "Selecciona una categoría.";
                return false;
            }

            int libros = repoLibros.CountByCategoria(idCategoria.Value);
            if (libros > 0)
            {
                error = $"No se puede borrar: esta categoría tiene {libros} libro(s) asociado(s).";
                return false;
            }

            error = null;
            return true;
        }
    }
}
