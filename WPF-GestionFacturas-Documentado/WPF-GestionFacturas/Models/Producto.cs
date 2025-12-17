using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_GestionFacturas.Models
{
    /// <summary>
    /// Representa un artículo o servicio disponible para la venta en el catálogo.
    /// <para>
    /// Esta clase mapea la tabla <c>Productos</c>. Se utiliza tanto para la gestión de inventario
    /// como para la selección de artículos al crear una factura.
    /// </para>
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Obtiene o establece el identificador único del producto.
        /// </summary>
        /// <value>
        /// Clave Primaria (PK) autoincremental en la base de datos.
        /// </value>
        public int IdProducto { get; set; }

        /// <summary>
        /// Obtiene o establece el código de referencia interna (SKU).
        /// </summary>
        /// <remarks>
        /// Se recomienda que este valor sea único para facilitar la búsqueda (ej: "REF-001").
        /// </remarks>
        public string Codigo { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre comercial del producto.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece una descripción detallada del producto.
        /// </summary>
        /// <value>
        /// Texto opcional con especificaciones técnicas o detalles adicionales.
        /// </value>
        public string Descripcion { get; set; }

        /// <summary>
        /// Obtiene o establece el precio de venta base actual.
        /// </summary>
        /// <remarks>
        /// Este es el precio sugerido que se copiará a la línea de detalle al crear una factura.
        /// </remarks>
        public decimal PrecioVenta { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad física disponible en el almacén.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Indica si el producto está disponible para la venta.
        /// </summary>
        /// <remarks>
        /// <b>Borrado Lógico:</b> En lugar de eliminar productos de la base de datos (lo que rompería facturas antiguas),
        /// se establece esta propiedad en <c>false</c> para ocultarlos en los selectores de nuevas facturas.
        /// </remarks>
        public bool Activo { get; set; }

        // --- PROPIEDADES DE PRESENTACIÓN (UI) ---

        /// <summary>
        /// Obtiene una cadena formateada con el nombre y el precio del producto.
        /// </summary>
        /// <remarks>
        /// <b>Propiedad de Solo Lectura:</b> No se guarda en base de datos.
        /// Está diseñada específicamente para mostrar información útil dentro de controles <c>ComboBox</c>
        /// o listas de selección rápida.
        /// </remarks>
        /// <example>
        /// Salida esperada: "Portátil HP - 550,00"
        /// </example>
        public string InfoProducto
        {
            get { return $"{Nombre} - {PrecioVenta} "; }
        }
    }
}