using P2526_Irene_Biblioteca.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace P2526_Irene_Biblioteca.Repositories
{
    public class LibrosRepository
    {
        private readonly string connectionString;

        public LibrosRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        public List<Libro> GetAll()
        {
            var lista = new List<Libro>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
                            SELECT  l.idLibro, l.titulo, l.año, l.idAutor, l.idCategoria, l.stock,
                                    a.nombre AS AutorNombre,
                                    c.nombre AS CategoriaNombre
                            FROM Libros l
                            INNER JOIN Autores a ON a.idAutor = l.idAutor
                            INNER JOIN Categorias c ON c.idCategoria = l.idCategoria
                            ORDER BY l.idLibro DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Libro
                        {
                            IdLibro = (int)reader["idLibro"],
                            Titulo = reader["titulo"].ToString(),
                            Año = (int)reader["año"],
                            IdAutor = (int)reader["idAutor"],
                            IdCategoria = (int)reader["idCategoria"],
                            Stock = (int)reader["stock"],
                            AutorNombre = reader["AutorNombre"].ToString(),
                            CategoriaNombre = reader["CategoriaNombre"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void Insert(Libro l)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Libros (titulo, año, idAutor, idCategoria, stock)
                               VALUES (@Titulo, @Anio, @IdAutor, @IdCategoria, @Stock)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Titulo", l.Titulo);
                    cmd.Parameters.AddWithValue("@Anio", l.Año);
                    cmd.Parameters.AddWithValue("@IdAutor", l.IdAutor);
                    cmd.Parameters.AddWithValue("@IdCategoria", l.IdCategoria);
                    cmd.Parameters.AddWithValue("@Stock", l.Stock);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Libro l)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE Libros
                               SET titulo=@Titulo, año=@Anio, idAutor=@IdAutor, idCategoria=@IdCategoria, stock=@Stock
                               WHERE idLibro=@IdLibro";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Titulo", l.Titulo);
                    cmd.Parameters.AddWithValue("@Anio", l.Año);
                    cmd.Parameters.AddWithValue("@IdAutor", l.IdAutor);
                    cmd.Parameters.AddWithValue("@IdCategoria", l.IdCategoria);
                    cmd.Parameters.AddWithValue("@Stock", l.Stock);
                    cmd.Parameters.AddWithValue("@IdLibro", l.IdLibro);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int idLibro)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Libros WHERE idLibro=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idLibro);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int CountByAutor(int idAutor)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Libros WHERE idAutor=@IdAutor";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdAutor", idAutor);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public int CountByCategoria(int idCategoria)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Libros WHERE idCategoria=@IdCategoria";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public int GetStock(int idLibro)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT stock FROM Libros WHERE idLibro=@Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idLibro);
                    object v = cmd.ExecuteScalar();
                    return v == null ? 0 : (int)v;
                }
            }
        }

        public void UpdateStock(int idLibro, int nuevoStock)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Libros SET stock=@s WHERE idLibro=@Id", conn))
                {
                    cmd.Parameters.AddWithValue("@s", nuevoStock);
                    cmd.Parameters.AddWithValue("@Id", idLibro);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
