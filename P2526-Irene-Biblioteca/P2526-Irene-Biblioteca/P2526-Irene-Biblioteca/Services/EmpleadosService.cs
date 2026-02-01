using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2526_Irene_Biblioteca.Services
{
    public class EmpleadosService
    {
        private readonly PrestamosRepository repoPrestamos = new PrestamosRepository();
        public bool PuedeEditar()
        {
            return SesionActual.EsEmpleado;
        }

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

        public bool ValidarModificacion(string nombre, string usuario, int? idEmpleado, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idEmpleado == null || idEmpleado <= 0)
            {
                error = "Selecciona un empleado.";
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

        public bool ValidarBorrado(int? idEmpleado, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idEmpleado == null || idEmpleado <= 0)
            {
                error = "Selecciona un empleado.";
                return false;
            }

            int prestamos = repoPrestamos.CountByEmpleado(idEmpleado.Value);
            if (prestamos > 0)
            {
                error = $"No se puede borrar: este empleado tiene {prestamos} préstamo(s).";
                return false;
            }

            error = null;
            return true;
        }
    }
}

