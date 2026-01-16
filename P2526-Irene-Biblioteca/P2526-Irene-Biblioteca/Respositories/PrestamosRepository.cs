using P2526_Irene_Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace P2526_Irene_Biblioteca.Repositories
{
    public class PrestamosRepository
    {
        private readonly string connectionString;

        public PrestamosRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
        }

        public List<Prestamo> GetAll()
        {
            var lista = new List<Prestamo>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
                            SELECT  p.idPrestamo, p.idLibro, p.idCliente, p.idEmpleado,
                                    p.fechaPrestamo, p.fechaDevolucion, p.devuelto
                            FROM Prestamos p
                            ORDER BY p.idPrestamo DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(new Prestamo
                        {
                            IdPrestamo = (int)r["idPrestamo"],
                            IdLibro = (int)r["idLibro"],
                            IdCliente = (int)r["idCliente"],
                            IdEmpleado = (int)r["idEmpleado"],
                            FechaPrestamo = (DateTime)r["fechaPrestamo"],
                            FechaDevolucion = r["fechaDevolucion"] == DBNull.Value ? (DateTime?)null : (DateTime)r["fechaDevolucion"],
                            Devuelto = (bool)r["devuelto"]
                        });
                    }
                }
            }

            return lista;
        }

        public int Insert(Prestamo p)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
                            INSERT INTO Prestamos (idLibro, idCliente, idEmpleado, fechaPrestamo, fechaDevolucion, devuelto)
                            VALUES (@idLibro, @idCliente, @idEmpleado, @fechaPrestamo, @fechaDevolucion, @devuelto);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idLibro", p.IdLibro);
                    cmd.Parameters.AddWithValue("@idCliente", p.IdCliente);
                    cmd.Parameters.AddWithValue("@idEmpleado", p.IdEmpleado);
                    cmd.Parameters.AddWithValue("@fechaPrestamo", p.FechaPrestamo);
                    cmd.Parameters.AddWithValue("@fechaDevolucion", (object)p.FechaDevolucion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@devuelto", p.Devuelto);

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Prestamo p)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
                            UPDATE Prestamos
                            SET idLibro=@idLibro,
                                idCliente=@idCliente,
                                idEmpleado=@idEmpleado,
                                fechaPrestamo=@fechaPrestamo,
                                fechaDevolucion=@fechaDevolucion,
                                devuelto=@devuelto
                            WHERE idPrestamo=@idPrestamo;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idPrestamo", p.IdPrestamo);
                    cmd.Parameters.AddWithValue("@idLibro", p.IdLibro);
                    cmd.Parameters.AddWithValue("@idCliente", p.IdCliente);
                    cmd.Parameters.AddWithValue("@idEmpleado", p.IdEmpleado);
                    cmd.Parameters.AddWithValue("@fechaPrestamo", p.FechaPrestamo);
                    cmd.Parameters.AddWithValue("@fechaDevolucion", (object)p.FechaDevolucion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@devuelto", p.Devuelto);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int idPrestamo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Prestamos WHERE idPrestamo=@Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idPrestamo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int CountByCliente(int idCliente)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Prestamos WHERE idCliente=@Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idCliente);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public int CountByEmpleado(int idEmpleado)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Prestamos WHERE idEmpleado=@Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idEmpleado);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public int CountByLibro(int idLibro)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Prestamos WHERE idLibro=@Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idLibro);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
