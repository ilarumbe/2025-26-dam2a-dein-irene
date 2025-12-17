using System;
using System.Collections.Generic;
using System.Windows; // Para MessageBox
using System.Windows.Input;
using WPF_GestionFacturas.Models;
using WPF_GestionFacturas.Repositories;
using WPF_GestionFacturas.Views;

namespace WPF_GestionFacturas.ViewModels
{
    /// <summary>
    /// ViewModel principal de la aplicación. Gestiona la lógica de la ventana <see cref="MainWindow"/>.
    /// <para>
    /// Actúa como intermediario entre la Vista (XAML) y el Modelo (Repositorios/Clases).
    /// Se encarga de exponer la lista de facturas, gestionar la selección del usuario y ejecutar 
    /// los comandos de impresión.
    /// </para>
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Instancia del repositorio para acceder a la base de datos de facturas.
        /// </summary>
        private FacturaRepository _repo = new FacturaRepository();

        // --- PROPIEDADES (DATOS PARA LA VISTA) ---

        private List<Factura> _listaFacturas;

        /// <summary>
        /// Obtiene o establece la colección de facturas que se mostrará en la tabla (DataGrid).
        /// </summary>
        /// <remarks>
        /// Al modificar esta propiedad, se lanza el evento <c>OnPropertyChanged</c> para actualizar la interfaz automáticamente.
        /// </remarks>
        public List<Factura> ListaFacturas
        {
            get { return _listaFacturas; }
            set { _listaFacturas = value; OnPropertyChanged(); }
        }

        private Factura _facturaSeleccionada;

        /// <summary>
        /// Obtiene o establece la factura que el usuario ha seleccionado en la interfaz.
        /// </summary>
        /// <value>
        /// Objeto <see cref="Factura"/> seleccionado o <c>null</c> si no hay selección.
        /// </value>
        /// <remarks>
        /// Esta propiedad está enlazada al <c>SelectedItem</c> del DataGrid. Es necesaria para saber
        /// qué factura imprimir cuando se pulsa el botón de "Imprimir Seleccionada".
        /// </remarks>
        public Factura FacturaSeleccionada
        {
            get { return _facturaSeleccionada; }
            set
            {
                _facturaSeleccionada = value;
                OnPropertyChanged();
            }
        }

        private string _textoEstado;

        /// <summary>
        /// Obtiene o establece el mensaje que se muestra en la barra de estado inferior.
        /// </summary>
        /// <example>"Se han cargado 50 facturas correctamente."</example>
        public string TextoEstado
        {
            get { return _textoEstado; }
            set { _textoEstado = value; OnPropertyChanged(); }
        }

        // --- COMANDOS (ACCIONES PARA LOS BOTONES) ---

        /// <summary>
        /// Comando para generar y visualizar el listado global de facturas.
        /// </summary>
        public ICommand ImprimirCommand { get; set; }

        /// <summary>
        /// Comando para generar y visualizar el informe detallado de la factura seleccionada.
        /// </summary>
        public ICommand ImprimirIndividualCommand { get; set; }

        // --- CONSTRUCTOR ---

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MainViewModel"/>.
        /// </summary>
        /// <remarks>
        /// Configura los comandos (RelayCommand) y realiza la carga inicial de datos desde la base de datos.
        /// </remarks>
        public MainViewModel()
        {
            // Comando para el listado general
            ImprimirCommand = new RelayCommand(param => AbrirInformeGeneral());

            // [NUEVO] Comando para imprimir la factura seleccionada
            ImprimirIndividualCommand = new RelayCommand(param => ImprimirFacturaActual());

            // Cargamos datos al iniciar
            CargarDatos();
        }

        // --- MÉTODOS ---

        /// <summary>
        /// Recupera los datos desde el repositorio y actualiza la lista de la interfaz.
        /// </summary>
        /// <exception cref="Exception">Captura cualquier error de conexión y lo muestra en la barra de estado.</exception>
        private void CargarDatos()
        {
            try
            {
                ListaFacturas = _repo.GetAll();
                TextoEstado = $"Se han cargado {ListaFacturas.Count} facturas correctamente.";
            }
            catch (Exception ex)
            {
                TextoEstado = "Error al cargar datos.";
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Abre la ventana <see cref="VisorInforme"/> para mostrar el reporte general.
        /// </summary>
        private void AbrirInformeGeneral()
        {
            VisorInforme ventana = new VisorInforme();
            ventana.ShowDialog();
        }

        /// <summary>
        /// Lógica para imprimir una factura individual.
        /// </summary>
        /// <remarks>
        /// Realiza tres pasos:
        /// <list type="number">
        /// <item><description>Valida que haya una <see cref="FacturaSeleccionada"/>.</description></item>
        /// <item><description>Obtiene las líneas de detalle desde la base de datos usando <see cref="FacturaRepository.GetDetallesPorFactura"/>.</description></item>
        /// <item><description>Abre la ventana <see cref="VisorFacturaIndividual"/> inyectando la cabecera y los detalles.</description></item>
        /// </list>
        /// </remarks>
        private void ImprimirFacturaActual()
        {
            // Validamos que haya algo seleccionado
            if (FacturaSeleccionada == null)
            {
                MessageBox.Show("Por favor, selecciona una factura de la lista para imprimir.",
                                "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Recuperamos las líneas de detalle de esa factura desde la BBDD
                var detalles = _repo.GetDetallesPorFactura(FacturaSeleccionada.IdFactura);

                // Abrimos el visor específico pasándole la Cabecera y los Detalles
                VisorFacturaIndividual ventana = new VisorFacturaIndividual(FacturaSeleccionada, detalles);
                ventana.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al preparar la factura: " + ex.Message);
            }
        }
    }
}