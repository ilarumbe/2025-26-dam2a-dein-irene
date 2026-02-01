using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace P2526_Irene_Biblioteca.Repositories
{
    public class EmpleadosRepository
    {
        private readonly string connectionString;

        public EmpleadosRepository()
        {
            connectionString = ConfigurationManager
                .ConnectionStrings["Conexion"]
                .ConnectionString;
        }

        public Empleado GetByUsuarioAndPassword(string usuario, string plainPassword)
        {
            string passwordHash = Helper.HashPassword(plainPassword);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"SELECT idEmpleado, nombre, usuario, password
                               FROM Empleados
                               WHERE usuario = @Usuario AND password = @Pass";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", usuario);
                    cmd.Parameters.AddWithValue("@Pass", passwordHash);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Empleado
                            {
                                IdEmpleado = (int)reader["idEmpleado"],
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

        public void Insert(Empleado e, string plainPassword)
        {
            string passwordHash = Helper.HashPassword(plainPassword);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"INSERT INTO Empleados (nombre, usuario, password)
                               VALUES (@Nombre, @Usuario, @Pass)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@Usuario", e.Usuario);
                    cmd.Parameters.AddWithValue("@Pass", passwordHash);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Empleado> GetAll()
        {
            var lista = new List<Empleado>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "SELECT idEmpleado, nombre, usuario FROM Empleados";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = (int)reader["idEmpleado"],
                            Nombre = reader["nombre"].ToString(),
                            Usuario = reader["usuario"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void Update(Empleado e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE Empleados
                               SET nombre = @Nombre,
                                   usuario = @Usuario
                               WHERE idEmpleado = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@Usuario", e.Usuario);
                    cmd.Parameters.AddWithValue("@Id", e.IdEmpleado);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePassword(int idEmpleado, string plainPassword)
        {
            string passwordHash = Helper.HashPassword(plainPassword);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"UPDATE Empleados
                               SET password = @Pass
                               WHERE idEmpleado = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Pass", passwordHash);
                    cmd.Parameters.AddWithValue("@Id", idEmpleado);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int idEmpleado)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Empleados WHERE idEmpleado = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idEmpleado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
