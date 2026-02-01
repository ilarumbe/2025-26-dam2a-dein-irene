using P2526_Irene_Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace P2526_Irene_Biblioteca.Repositories
{
    /// <summary>
    /// Repositorio de acceso a datos para la entidad <see cref="Autor"/>.
    /// Centraliza las operaciones CRUD contra la tabla Autores.
    /// </summary>
    public class AutoresRepository
    {
        private readonly string connectionString;

        /// <summary>
        /// Inicializa el repositorio leyendo la cadena de conexión "Conexion" desde App.config.
        /// </summary>
        public AutoresRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        /// <summary>
        /// Obtiene todos los autores almacenados en la tabla Autores.
        /// </summary>
        /// <returns>
        /// Lista de <see cref="Autor"/> con sus campos IdAutor, Nombre y Nacionalidad.
        /// </returns>
        public List<Autor> GetAll()
        {
            var lista = new List<Autor>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT idAutor, nombre, nacionalidad FROM Autores";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Autor
                        {
                            IdAutor = (int)reader["idAutor"],
                            Nombre = reader["nombre"].ToString(),
                            Nacionalidad = reader["nacionalidad"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Inserta un nuevo autor en la tabla Autores.
        /// </summary>
        /// <param name="a">Autor a insertar. Debe contener al menos <see cref="Autor.Nombre"/>.</param>
        /// <exception cref="ArgumentNullException">Se lanza si <paramref name="a"/> es null.</exception>
        public void Insert(Autor a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Autores (nombre, nacionalidad)
                               VALUES (@Nombre, @Nacionalidad)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", a.Nombre);
                    cmd.Parameters.AddWithValue("@Nacionalidad", (object)a.Nacionalidad ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Actualiza los datos de un autor existente en la tabla Autores.
        /// </summary>
        /// <param name="a">
        /// Autor a actualizar. Debe tener <see cref="Autor.IdAutor"/> válido y los campos a modificar.
        /// </param>
        /// <exception cref="ArgumentNullException">Se lanza si <paramref name="a"/> es null.</exception>
        public void Update(Autor a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE Autores
                               SET nombre = @Nombre,
                                   nacionalidad = @Nacionalidad
                               WHERE idAutor = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", a.Nombre);
                    cmd.Parameters.AddWithValue("@Nacionalidad", (object)a.Nacionalidad ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", a.IdAutor);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina un autor por su identificador.
        /// </summary>
        /// <param name="idAutor">Identificador del autor a eliminar.</param>
        public void Delete(int idAutor)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Autores WHERE idAutor = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idAutor);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
