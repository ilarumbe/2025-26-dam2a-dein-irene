using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_GestionFacturas.Models
{
    /// <summary>
    /// Representa una línea individual (ítem) dentro de una factura.
    /// <para>
    /// Mapea la tabla <c>FacturasDetalle</c>. Cada instancia corresponde a un producto específico 
    /// vendido dentro de una operación, guardando las condiciones económicas del momento (precio histórico).
    /// </para>
    /// </summary>
    public class FacturaDetalle
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la línea de detalle.
        /// </summary>
        /// <value>
        /// Clave Primaria (PK) autoincremental en la base de datos.
        /// </value>
        public int IdDetalle { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la factura padre.
        /// </summary>
        /// <value>
        /// Clave Foránea (FK) que vincula esta línea con la cabecera en la tabla <c>Facturas</c>.
        /// </value>
        public int IdFactura { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del producto vendido.
        /// </summary>
        /// <value>
        /// Clave Foránea (FK) a la tabla <c>Productos</c>.
        /// </value>
        public int IdProducto { get; set; }

        /// <summary>
        /// Obtiene o establece el número de unidades vendidas.
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Obtiene o establece el precio unitario del producto en el momento de la venta.
        /// </summary>
        /// <remarks>
        /// <b>Importante:</b> Este valor es una "instantánea" (Snapshot). 
        /// No debe leerse directamente de la tabla Productos, ya que si el precio del catálogo cambia en el futuro,
        /// esta factura histórica no debe verse alterada.
        /// </remarks>
        public decimal PrecioUnitario { get; set; }

        // --- PROPIEDADES CALCULADAS Y EXTENDIDAS ---

        /// <summary>
        /// Obtiene el importe total de la línea.
        /// </summary>
        /// <value>
        /// Resultado del cálculo matemático: <c>Cantidad * PrecioUnitario</c>.
        /// </value>
        /// <remarks>
        /// Esta propiedad es de solo lectura y se calcula en tiempo de ejecución en la aplicación (C#),
        /// aunque en la base de datos también puede existir como columna computada.
        /// </remarks>
        public decimal Subtotal
        {
            get { return Cantidad * PrecioUnitario; }
        }

        /// <summary>
        /// Obtiene o establece el nombre descriptivo del producto.
        /// </summary>
        /// <remarks>
        /// <b>Propiedad Extendida:</b> No pertenece a la tabla <c>FacturasDetalle</c>.
        /// Se obtiene mediante una consulta <c>INNER JOIN</c> con la tabla <c>Productos</c> para facilitar
        /// la visualización en los informes sin consultas adicionales.
        /// </remarks>
        public string NombreProducto { get; set; }
    }
}