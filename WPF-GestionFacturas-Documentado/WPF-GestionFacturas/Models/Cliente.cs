using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_GestionFacturas.Models
{
    /// <summary>
    /// Representa a un cliente dentro del sistema de facturación.
    /// <para>
    /// Esta clase mapea la estructura de la tabla <c>Clientes</c> de la base de datos.
    /// Contiene la información fiscal y de contacto necesaria para emitir facturas.
    /// </para>
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Obtiene o establece el identificador único del cliente.
        /// </summary>
        /// <value>
        /// Un entero que actúa como Clave Primaria (PK) en la base de datos.
        /// Es un valor autoincremental generado por SQL Server.
        /// </value>
        public int IdCliente { get; set; }

        /// <summary>
        /// Obtiene o establece el Número de Identificación Fiscal (DNI, NIF, CIF).
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio y debería ser único en el sistema para evitar duplicidad de clientes.
        /// </remarks>
        public string NIF { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre completo (personas físicas) o la razón social (empresas).
        /// </summary>
        public string NombreRazonSocial { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección física o fiscal del cliente.
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Obtiene o establece el número de teléfono de contacto.
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección de correo electrónico.
        /// </summary>
        /// <remarks>
        /// Se utiliza como dato de contacto principal y para el envío digital de facturas.
        /// </remarks>
        public string Email { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en la que el cliente fue registrado en el sistema.
        /// </summary>
        public DateTime FechaAlta { get; set; }
    }
}