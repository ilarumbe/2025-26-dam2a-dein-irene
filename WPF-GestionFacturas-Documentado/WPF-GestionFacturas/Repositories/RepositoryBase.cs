using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace WPF_GestionFacturas.Repositories
{
    /// <summary>
    /// Clase base abstracta para todos los repositorios de la aplicación.
    /// <para>
    /// Centraliza la lógica de conexión a la base de datos SQL Server. 
    /// Se encarga de leer la cadena de conexión del archivo de configuración (<c>App.config</c>) 
    /// y proveer objetos de conexión listos para usar a las clases derivadas.
    /// </para>
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// Almacena la cadena de conexión recuperada del archivo de configuración.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RepositoryBase"/>.
        /// </summary>
        /// <remarks>
        /// Durante la construcción, intenta leer la cadena de conexión llamada "ConexionFacturacion"
        /// de la sección <c>connectionStrings</c> del archivo <c>App.config</c>.
        /// </remarks>
        /// <exception cref="ConfigurationErrorsException">Se lanza si no se encuentra la cadena de conexión especificada.</exception>
        public RepositoryBase()
        {
            // Leemos la cadena llamada "ConexionFacturacion" del App.config
            _connectionString = ConfigurationManager.ConnectionStrings["ConexionFacturacion"].ConnectionString;
        }

        /// <summary>
        /// Crea y devuelve una nueva instancia de conexión a la base de datos.
        /// </summary>
        /// <remarks>
        /// Este método es <c>protected</c>, por lo que solo puede ser utilizado por las clases que hereden de <see cref="RepositoryBase"/>
        /// (como <see cref="FacturaRepository"/>).
        /// </remarks>
        /// <returns>
        /// Un objeto <see cref="SqlConnection"/> inicializado con la cadena de conexión configurada.
        /// </returns>
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}