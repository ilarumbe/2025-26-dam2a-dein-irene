using P2526_Irene_Biblioteca.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace P2526_Irene_Biblioteca.Repositories
{
    public class CategoriasRepository
    {
        private readonly string connectionString;

        public CategoriasRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        public List<Categoria> GetAll()
        {
            var lista = new List<Categoria>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT idCategoria, nombre FROM Categorias";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Categoria
                        {
                            IdCategoria = (int)reader["idCategoria"],
                            Nombre = reader["nombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void Insert(Categoria c)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Categorias (nombre)
                               VALUES (@Nombre)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Categoria c)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE Categorias
                               SET nombre = @Nombre
                               WHERE idCategoria = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                    cmd.Parameters.AddWithValue("@Id", c.IdCategoria);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int idCategoria)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Categorias WHERE idCategoria = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idCategoria);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
