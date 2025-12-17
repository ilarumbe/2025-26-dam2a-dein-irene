using System;
using System.Collections.Generic; // Para usar List<T>
using Microsoft.Data.SqlClient;
using WPF_GestionFacturas.Models;

namespace WPF_GestionFacturas.Repositories
{
    /// <summary>
    /// Gestiona el acceso a datos para las tablas <c>Facturas</c> y <c>FacturasDetalle</c>.
    /// <para>
    /// Esta clase se encarga de ejecutar las consultas SQL para recuperar listados generales
    /// y detalles específicos necesarios para la generación de informes.
    /// </para>
    /// </summary>
    /// <seealso cref="RepositoryBase"/>
    public class FacturaRepository : RepositoryBase
    {
        /// <summary>
        /// Obtiene el listado completo de todas las facturas registradas en el sistema.
        /// </summary>
        /// <remarks>
        /// Esta consulta realiza dos operaciones clave:
        /// <list type="bullet">
        /// <item>
        ///     <description><b>INNER JOIN con Clientes:</b> Para recuperar el <c>NombreRazonSocial</c> y el <c>NIF</c> sin consultas adicionales.</description>
        /// </item>
        /// <item>
        ///     <description><b>Subconsulta de Totales:</b> Calcula dinámicamente el total de la factura sumando (<c>Cantidad * Precio</c>) de la tabla de detalles.</description>
        /// </item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// Una lista de objetos <see cref="Factura"/> ordenada por fecha descendente (las más recientes primero).
        /// </returns>
        /// <exception cref="SqlException">Se lanza si hay errores de conexión o sintaxis SQL.</exception>
        public List<Factura> GetAll()
        {
            List<Factura> lista = new List<Factura>();

            using (var conn = GetConnection())
            {
                conn.Open();

                // CAMBIO EN LA QUERY:
                // Añadimos una subconsulta para sumar (Cantidad * Precio) de la tabla Detalle
                string query = @"
            SELECT 
                f.IdFactura, 
                f.NumeroFactura, 
                f.Fecha, 
                f.Observaciones,
                c.NombreRazonSocial,
                c.NIF,
                c.IdCliente,
                -- ESTA ES LA LÍNEA NUEVA QUE CALCULA EL TOTAL:
                (SELECT ISNULL(SUM(d.Cantidad * d.PrecioUnitario), 0) 
                 FROM FacturasDetalle d 
                 WHERE d.IdFactura = f.IdFactura) AS TotalCalculado

            FROM Facturas f
            INNER JOIN Clientes c ON f.IdCliente = c.IdCliente
            ORDER BY f.Fecha DESC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var factura = new Factura
                            {
                                IdFactura = (int)reader["IdFactura"],
                                IdCliente = (int)reader["IdCliente"],
                                NumeroFactura = reader["NumeroFactura"].ToString(),
                                Fecha = (DateTime)reader["Fecha"],
                                Observaciones = reader["Observaciones"] == DBNull.Value ? "" : reader["Observaciones"].ToString(),
                                NombreCliente = reader["NombreRazonSocial"].ToString(),
                                NIFCliente = reader["NIF"].ToString(),
                                // AHORA SÍ LEEMOS EL TOTAL:
                                TotalFactura = (decimal)reader["TotalCalculado"]
                            };

                            lista.Add(factura);
                        }
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Recupera las líneas de detalle (productos vendidos) asociadas a una factura específica.
        /// </summary>
        /// <param name="idFactura">El identificador único (PK) de la factura cabecera.</param>
        /// <remarks>
        /// Realiza un <c>INNER JOIN</c> con la tabla <c>Productos</c> para obtener el nombre del artículo
        /// en el momento de la consulta.
        /// </remarks>
        /// <returns>
        /// Una lista de objetos <see cref="FacturaDetalle"/> que componen el cuerpo de la factura.
        /// </returns>
        public List<FacturaDetalle> GetDetallesPorFactura(int idFactura)
        {
            List<FacturaDetalle> lista = new List<FacturaDetalle>();

            using (var conn = GetConnection())
            {
                conn.Open();

                // JOIN con Productos para saber el nombre de lo que se vendió
                string query = @"
                    SELECT 
                        fd.IdDetalle, fd.IdFactura, fd.IdProducto, fd.Cantidad, fd.PrecioUnitario,
                        p.Nombre as NombreProducto
                    FROM FacturasDetalle fd
                    INNER JOIN Productos p ON fd.IdProducto = p.IdProducto
                    WHERE fd.IdFactura = @IdFactura";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdFactura", idFactura);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new FacturaDetalle
                            {
                                IdDetalle = (int)reader["IdDetalle"],
                                IdFactura = (int)reader["IdFactura"],
                                IdProducto = (int)reader["IdProducto"],
                                Cantidad = (int)reader["Cantidad"],
                                PrecioUnitario = (decimal)reader["PrecioUnitario"],
                                NombreProducto = reader["NombreProducto"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}