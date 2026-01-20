using System;

namespace P2526_Irene_Biblioteca.Models
{
    /// <summary>
    /// Representa un autor de la biblioteca.
    /// Contiene los datos básicos que se almacenan en la tabla Autores.
    /// </summary>
    public class Autor
    {
        /// <summary>
        /// Identificador único del autor (clave primaria en BBDD).
        /// </summary>
        public int IdAutor { get; set; }

        /// <summary>
        /// Nombre del autor.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Nacionalidad del autor (opcional).
        /// </summary>
        public string Nacionalidad { get; set; }

        /// <summary>
        /// Crea un autor vacío. Útil para binding o inicializaciones.
        /// </summary>
        public Autor() { }

        /// <summary>
        /// Crea un autor con sus datos principales.
        /// </summary>
        /// <param name="idAutor">Identificador del autor.</param>
        /// <param name="nombre">Nombre del autor.</param>
        /// <param name="nacionalidad">Nacionalidad del autor (puede ser null).</param>
        public Autor(int idAutor, string nombre, string nacionalidad = null)
        {
            IdAutor = idAutor;
            Nombre = nombre;
            Nacionalidad = nacionalidad;
        }

        /// <summary>
        /// Devuelve una representación legible del autor para depuración o logs.
        /// </summary>
        /// <returns>Una cadena con el nombre y la nacionalidad si existe.</returns>
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Nacionalidad)
                ? Nombre
                : $"{Nombre} ({Nacionalidad})";
        }
    }
}
