using P2526_Irene_Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Empleado Login(string usuario, string passwordHash)
        {
            using (var cn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Empleados WHERE usuario=@u AND password=@p";
                var cmd = new SqlCommand(sql, cn);

                cmd.Parameters.AddWithValue("@u", usuario);
                cmd.Parameters.AddWithValue("@p", passwordHash);

                cn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Empleado
                    {
                        IdEmpleado = (int)reader["idEmpleado"],
                        Nombre = reader["nombre"].ToString(),
                        Usuario = reader["usuario"].ToString()
                    };
                }
            }
            return null;
        }
    }
}
