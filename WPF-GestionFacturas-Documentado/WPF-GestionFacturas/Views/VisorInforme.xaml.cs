using System;
using System.Windows;
using Microsoft.Reporting.WinForms; // Necesario para ReportDataSource
using WPF_GestionFacturas.Repositories;

namespace WPF_GestionFacturas.Views
{
    /// <summary>
    /// Lógica de interacción para la ventana de visualización del informe general de facturas.
    /// <para>
    /// Esta ventana contiene un control <see cref="ReportViewer"/> encargado de renderizar el archivo 'ListadoFacturas.rdlc'.
    /// Se utiliza para mostrar el listado completo de facturas existentes en la base de datos, incluyendo los totales calculados.
    /// </para>
    /// </summary>
    public partial class VisorInforme : Window
    {
        /// <summary>
        /// Inicializa una nueva instancia de la ventana <see cref="VisorInforme"/>.
        /// </summary>
        /// <remarks>
        /// Al instanciarse, recupera automáticamente los datos de la base de datos y genera el informe
        /// llamando al método interno <see cref="CargarInforme"/>.
        /// </remarks>
        public VisorInforme()
        {
            InitializeComponent(); // ¡No borres esto!
            CargarInforme();
        }

        /// <summary>
        /// Orquesta la carga de datos y la configuración del visor de reportes.
        /// </summary>
        /// <remarks>
        /// Realiza los siguientes pasos secuenciales:
        /// <list type="number">
        /// <item><description>Instancia el repositorio <see cref="FacturaRepository"/> y recupera todas las facturas mediante <see cref="FacturaRepository.GetAll"/>.</description></item>
        /// <item><description>Calcula la ruta física del archivo de definición del reporte (<c>ListadoFacturas.rdlc</c>).</description></item>
        /// <item><description>Crea el origen de datos (<see cref="ReportDataSource"/>) vinculando la lista de facturas con el nombre clave "DataSetFacturas".</description></item>
        /// <item><description>Refresca el visor para renderizar el documento final.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="Exception">Captura errores de Entrada/Salida (archivo no encontrado) o errores de conexión a base de datos y los muestra al usuario.</exception>
        private void CargarInforme()
        {
            try
            {
                // Obtener datos de la base de datos
                var repo = new FacturaRepository();
                var datos = repo.GetAll();

                // Calcular la ruta del archivo .rdlc (ruta segura)
                string rutaEjecutable = AppDomain.CurrentDomain.BaseDirectory;
                string rutaInforme = System.IO.Path.Combine(rutaEjecutable, "Reports", "ListadoFacturas.rdlc");

                // Verificación de seguridad
                if (!System.IO.File.Exists(rutaInforme))
                {
                    MessageBox.Show("No encuentro el archivo .rdlc en:\n" + rutaInforme);
                    return;
                }

                // Configurar el Visor
                visorFacturas.LocalReport.ReportPath = rutaInforme;
                visorFacturas.LocalReport.DataSources.Clear();

                // DATASET (Líneas)
                // "DataSetFacturas" debe ser el nombre del DataSet en el RDLC
                var fuenteDatos = new ReportDataSource("DataSetFacturas", datos);
                visorFacturas.LocalReport.DataSources.Add(fuenteDatos);

                // Refrescar
                visorFacturas.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el informe: " + ex.Message);
            }
        }

    }
}