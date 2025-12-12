using System.Configuration;
using System.Data.SqlClient;
using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Models;

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
    }
}
