using P2526_Irene_Biblioteca.Helpers;

namespace P2526_Irene_Biblioteca.Services
{
    public class AutoresService
    {
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
                error = "El nombre del autor es obligatorio.";
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidarModificacion(int? idAutor, string nombre, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idAutor == null || idAutor <= 0)
            {
                error = "Selecciona un autor.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                error = "El nombre del autor es obligatorio.";
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidarBorrado(int? idAutor, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idAutor == null || idAutor <= 0)
            {
                error = "Selecciona un autor.";
                return false;
            }

            error = null;
            return true;
        }
    }
}

