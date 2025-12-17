using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Windows;
using WPF_GestionFacturas.Models;

namespace WPF_GestionFacturas.Views
{
    /// <summary>
    /// Lógica de interacción para la ventana de visualización de una factura individual.
    /// <para>
    /// Esta ventana aloja el control ReportViewer configurado para mostrar el reporte 'FacturaIndividual.rdlc'.
    /// A diferencia del listado general, esta ventana requiere recibir los datos específicos (cabecera y líneas)
    /// desde el ViewModel antes de mostrarse.
    /// </para>
    /// </summary>
    public partial class VisorFacturaIndividual : Window
    {
        /// <summary>
        /// Inicializa una nueva instancia de la ventana <see cref="VisorFacturaIndividual"/> cargando los datos proporcionados.
        /// </summary>
        /// <param name="cabecera">
        /// Objeto <see cref="Factura"/> que contiene los datos principales (Cliente, Fecha, NIF, Total).
        /// Estos datos se mapearán a los <b>Parámetros</b> del informe.
        /// </param>
        /// <param name="lineas">
        /// Lista de objetos <see cref="FacturaDetalle"/> que contiene los productos vendidos en esta operación.
        /// Estos datos se mapearán al <b>DataSet</b> del informe para generar la tabla de detalle.
        /// </param>
        public VisorFacturaIndividual(Factura cabecera, List<FacturaDetalle> lineas)
        {
            InitializeComponent();
            CargarInforme(cabecera, lineas);
        }

        /// <summary>
        /// Configura el visor de informes, asigna la ruta del archivo .rdlc e inyecta los datos.
        /// </summary>
        /// <param name="cabecera">Datos generales de la factura.</param>
        /// <param name="lineas">Listado de productos de la factura.</param>
        /// <remarks>
        /// Este método realiza las siguientes acciones críticas:
        /// <list type="number">
        /// <item><description>Localiza el archivo <c>FacturaIndividual.rdlc</c> en la carpeta <c>Reports</c>.</description></item>
        /// <item><description>Limpia cualquier origen de datos previo del visor.</description></item>
        /// <item><description>Crea y asigna los <see cref="ReportParameter"/> para la cabecera del documento.</description></item>
        /// <item><description>Crea y asigna el <see cref="ReportDataSource"/> con el nombre clave "DataSetDetalles".</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="Exception">Captura y muestra cualquier error relacionado con la carga del archivo o la asignación de datos.</exception>
        private void CargarInforme(Factura cabecera, List<FacturaDetalle> lineas)
        {
            try
            {
                // Construcción de la ruta absoluta al archivo del informe
                string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "FacturaIndividual.rdlc");

                visor.LocalReport.ReportPath = ruta;
                visor.LocalReport.DataSources.Clear();

                // 1. CONFIGURACIÓN DE PARÁMETROS (Datos de Cabecera)
                // Es vital que los nombres ("pNumeroFactura", "pCliente", etc.) coincidan con los definidos en el diseño RDLC.
                ReportParameter[] parametros = new ReportParameter[]
                {
                    new ReportParameter("pNumeroFactura", cabecera.NumeroFactura),
                    new ReportParameter("pFecha", cabecera.Fecha.ToShortDateString()),
                    new ReportParameter("pCliente", cabecera.NombreCliente),
                    new ReportParameter("pNIF", cabecera.NIFCliente),
                    new ReportParameter("pTotal", cabecera.TotalFactura.ToString("C2")) // Formato moneda según cultura local
                };
                visor.LocalReport.SetParameters(parametros);

                // 2. CONFIGURACIÓN DEL DATASET (Líneas de Detalle)
                // "DataSetDetalles" es el nombre del conjunto de datos definido dentro del archivo .rdlc
                var fuenteDatos = new ReportDataSource("DataSetDetalles", lineas);
                visor.LocalReport.DataSources.Add(fuenteDatos);

                // Renderizar el informe
                visor.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el informe individual: " + ex.Message, "Error de Reporte", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}