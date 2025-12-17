using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_GestionFacturas.Models
{
    /// <summary>
    /// Representa la cabecera de un documento de facturación.
    /// <para>
    /// Esta clase mapea la tabla <c>Facturas</c> de la base de datos, pero también actúa como un DTO (Data Transfer Object)
    /// al incluir propiedades extendidas con datos del cliente y totales calculados.
    /// </para>
    /// </summary>
    public class Factura
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la factura.
        /// </summary>
        /// <value>
        /// Clave Primaria (PK) autoincremental de la base de datos.
        /// </value>
        public int IdFactura { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del cliente al que pertenece la factura.
        /// </summary>
        /// <value>
        /// Clave Foránea (FK) que referencia a la tabla <c>Clientes</c>.
        /// </value>
        public int IdCliente { get; set; }

        /// <summary>
        /// Obtiene o establece el código visual o número de serie de la factura.
        /// <example>Ejemplo: "2025/001"</example>
        /// </summary>
        public string NumeroFactura { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de emisión de la factura.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Obtiene o establece notas internas o comentarios visibles en el documento.
        /// </summary>
        /// <remarks>
        /// Este campo es opcional y permite valores nulos o cadenas vacías.
        /// </remarks>
        public string Observaciones { get; set; }

        // --- PROPIEDADES EXTENDIDAS (NO ESTÁN EN LA TABLA FACTURAS) ---

        /// <summary>
        /// Obtiene o establece el NIF del cliente asociado.
        /// </summary>
        /// <remarks>
        /// <b>Propiedad Extendida:</b> Este valor no existe en la tabla <c>Facturas</c>.
        /// Se obtiene mediante una consulta SQL con <c>INNER JOIN</c> a la tabla <c>Clientes</c>.
        /// </remarks>
        public string NIFCliente { get; set; }

        /// <summary>
        /// Obtiene o establece el Nombre o Razón Social del cliente asociado.
        /// </summary>
        /// <remarks>
        /// <b>Propiedad Extendida:</b> Obtenida mediante <c>INNER JOIN</c>.
        /// Se utiliza para mostrar el nombre en los listados sin tener que consultar la tabla de clientes por separado.
        /// </remarks>
        public string NombreCliente { get; set; }

        /// <summary>
        /// Obtiene o establece el importe total de la factura.
        /// </summary>
        /// <remarks>
        /// <b>Propiedad Calculada:</b> Este valor se genera en la consulta SQL mediante una subconsulta 
        /// que suma las líneas de detalle (<c>Cantidad * PrecioUnitario</c>) asociadas a este ID de factura.
        /// </remarks>
        public decimal TotalFactura { get; set; }
    }
}