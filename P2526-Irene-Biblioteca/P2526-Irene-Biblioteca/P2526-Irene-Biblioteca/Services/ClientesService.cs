using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Repositories;

namespace P2526_Irene_Biblioteca.Services
{
    public class ClientesService
    {
        private readonly PrestamosRepository repoPrestamos = new PrestamosRepository();
        public bool PuedeEditar() => SesionActual.EsEmpleado;

        public bool ValidarAlta(string nombre, string usuario, string password, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(password))
            {
                error = "Rellena Nombre, Usuario y Contraseña.";
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidarModificacion(int? idCliente, string nombre, string usuario, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idCliente == null || idCliente <= 0)
            {
                error = "Selecciona un cliente.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(usuario))
            {
                error = "Nombre y Usuario no pueden estar vacíos.";
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidarBorrado(int? idCliente, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idCliente == null || idCliente <= 0)
            {
                error = "Selecciona un cliente.";
                return false;
            }

            int prestamos = repoPrestamos.CountByCliente(idCliente.Value);
            if (prestamos > 0)
            {
                error = $"No se puede borrar: este cliente tiene {prestamos} préstamo(s).";
                return false;
            }

            error = null;
            return true;
        }
    }
}
