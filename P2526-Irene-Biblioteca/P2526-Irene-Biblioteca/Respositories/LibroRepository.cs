using P2526_Irene_Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2526_Irene_Biblioteca.Respositories
{
    public class LibrosRepository
    {
        private readonly string connectionString;

        public LibrosRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<Libro> GetAll()
        {
            var lista = new List<Libro>();

            using (var cn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Libros";
                var cmd = new SqlCommand(query, cn);
                cn.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Libro
                    {
                        IdLibro = (int)reader["idLibro"],
                        Titulo = reader["titulo"].ToString(),
                        Ano = (int)reader["ano"],
                        IdAutor = (int)reader["idAutor"],
                        IdCategoria = (int)reader["idCategoria"],
                        Stock = (int)reader["stock"]
                    });
                }
            }

            return lista;
        }

        public void Insert(Libro libro)
        {
            using (var cn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Libros(titulo,ano,idAutor,idCategoria,stock)
                           VALUES(@t,@a,@au,@c,@s)";
                var cmd = new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue("@t", libro.Titulo);
                cmd.Parameters.AddWithValue("@a", libro.Ano);
                cmd.Parameters.AddWithValue("@au", libro.IdAutor);
                cmd.Parameters.AddWithValue("@c", libro.IdCategoria);
                cmd.Parameters.AddWithValue("@s", libro.Stock);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Libro libro)
        {
            using (var cn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Libros SET titulo=@t, ano=@a, idAutor=@au,
                           idCategoria=@c, stock=@s WHERE idLibro=@id";

                var cmd = new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue("@id", libro.IdLibro);
                cmd.Parameters.AddWithValue("@t", libro.Titulo);
                cmd.Parameters.AddWithValue("@a", libro.Ano);
                cmd.Parameters.AddWithValue("@au", libro.IdAutor);
                cmd.Parameters.AddWithValue("@c", libro.IdCategoria);
                cmd.Parameters.AddWithValue("@s", libro.Stock);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var cn = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Libros WHERE idLibro=@id";
                var cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
