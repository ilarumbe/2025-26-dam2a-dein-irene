using P2526_Irene_Biblioteca.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace P2526_Irene_Biblioteca.Repositories
{
    public class AutoresRepository
    {
        private readonly string connectionString;

        public AutoresRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

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

        public void Insert(Autor a)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Autores (nombre, nacionalidad)
                               VALUES (@Nombre, @Nacionalidad)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", a.Nombre);
                    cmd.Parameters.AddWithValue("@Nacionalidad", (object)a.Nacionalidad ?? System.DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Autor a)
        {
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
                    cmd.Parameters.AddWithValue("@Nacionalidad", (object)a.Nacionalidad ?? System.DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", a.IdAutor);
                    cmd.ExecuteNonQuery();
                }
            }
        }

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

