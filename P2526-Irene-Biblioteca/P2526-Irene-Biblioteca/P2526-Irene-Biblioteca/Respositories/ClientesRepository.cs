using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace P2526_Irene_Biblioteca.Repositories
{
    public class ClientesRepository
    {
        private readonly string connectionString;

        public ClientesRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        public Cliente GetByUsuarioAndPassword(string usuario, string plainPassword)
        {
            string passwordHash = Helper.HashPassword(plainPassword);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT idCliente, nombre, usuario, password
                               FROM Clientes
                               WHERE usuario = @Usuario AND password = @Pass";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Pass", passwordHash);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                IdCliente = (int)reader["idCliente"],
                                Nombre = reader["nombre"].ToString(),
                                Usuario = reader["usuario"].ToString(),
                                Password = reader["password"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public Cliente GetByUsuario(string usuario)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT idCliente, nombre, usuario
                               FROM Clientes
                               WHERE usuario = @Usuario";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                IdCliente = (int)reader["idCliente"],
                                Nombre = reader["nombre"].ToString(),
                                Usuario = reader["usuario"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public List<Cliente> GetAll()
        {
            var lista = new List<Cliente>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT idCliente, nombre, usuario FROM Clientes";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Cliente
                        {
                            IdCliente = (int)reader["idCliente"],
                            Nombre = reader["nombre"].ToString(),
                            Usuario = reader["usuario"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void Insert(Cliente c, string plainPassword)
        {
            string passwordHash = Helper.HashPassword(plainPassword);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Clientes (nombre, usuario, password)
                               VALUES (@Nombre, @Usuario, @Pass)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                    cmd.Parameters.AddWithValue("@Usuario", c.Usuario);
                    cmd.Parameters.AddWithValue("@Pass", passwordHash);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Cliente c)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE Clientes
                               SET nombre=@Nombre, usuario=@Usuario
                               WHERE idCliente=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                    cmd.Parameters.AddWithValue("@Usuario", c.Usuario);
                    cmd.Parameters.AddWithValue("@Id", c.IdCliente);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePassword(int idCliente, string plainPassword)
        {
            string passwordHash = Helper.HashPassword(plainPassword);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE Clientes
                               SET password=@Pass
                               WHERE idCliente=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Pass", passwordHash);
                    cmd.Parameters.AddWithValue("@Id", idCliente);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int idCliente)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Clientes WHERE idCliente=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idCliente);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
