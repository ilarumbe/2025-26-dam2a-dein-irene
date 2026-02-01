using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Repositories;

namespace P2526_Irene_Biblioteca.Services
{
    public class LibrosService
    {
        private readonly PrestamosRepository repoPrestamos = new PrestamosRepository();
        public bool PuedeEditar() => SesionActual.EsEmpleado;

        public bool ValidarAlta(string titulo, string anioTxt, object autorSel, object catSel, string stockTxt, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (string.IsNullOrWhiteSpace(titulo))
            {
                error = "El título es obligatorio.";
                return false;
            }

            if (!int.TryParse(anioTxt, out int anio) || anio < 0)
            {
                error = "Año inválido.";
                return false;
            }

            if (autorSel == null)
            {
                error = "Selecciona un autor.";
                return false;
            }

            if (catSel == null)
            {
                error = "Selecciona una categoría.";
                return false;
            }

            if (!int.TryParse(stockTxt, out int stock) || stock < 0)
            {
                error = "Stock inválido (0 o más).";
                return false;
            }

            error = null;
            return true;
        }

        public bool ValidarModificacion(int? idLibro, string titulo, string anioTxt, object autorSel, object catSel, string stockTxt, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idLibro == null || idLibro <= 0)
            {
                error = "Selecciona un libro.";
                return false;
            }

            return ValidarAlta(titulo, anioTxt, autorSel, catSel, stockTxt, out error);
        }

        public bool ValidarBorrado(int? idLibro, out string error)
        {
            if (!PuedeEditar())
            {
                error = "No tienes permisos (solo empleados).";
                return false;
            }

            if (idLibro == null || idLibro <= 0)
            {
                error = "Selecciona un libro.";
                return false;
            }

            int prestamos = repoPrestamos.CountByLibro(idLibro.Value);
            if (prestamos > 0)
            {
                error = $"No se puede borrar: este libro tiene {prestamos} préstamo(s).";
                return false;
            }


            error = null;
            return true;
        }
    }
}
