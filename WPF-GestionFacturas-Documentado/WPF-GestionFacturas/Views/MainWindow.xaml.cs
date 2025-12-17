using System.Windows;
using WPF_GestionFacturas.ViewModels; // Importante

namespace WPF_GestionFacturas.Views
{
    /// <summary>
    /// Lógica de interacción para la ventana principal de la aplicación.
    /// <para>
    /// Esta clase representa la "Vista" (View) en el patrón MVVM. Su responsabilidad se limita a
    /// inicializar los componentes visuales definidos en XAML y establecer el contexto de datos.
    /// No debe contener lógica de negocio ni acceso a datos directo.
    /// </para>
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MainWindow"/>.
        /// </summary>
        /// <remarks>
        /// En el constructor se realiza la vinculación (Binding) fundamental del patrón MVVM:
        /// Se crea una instancia de <see cref="MainViewModel"/> y se asigna a la propiedad <see cref="FrameworkElement.DataContext"/>.
        /// Esto permite que todos los controles del XAML (DataGrid, Botones) puedan acceder a las propiedades y comandos del ViewModel.
        /// </remarks>
        public MainWindow()
        {
            InitializeComponent();

            // Conectamos la vista con su ViewModel
            this.DataContext = new MainViewModel();
        }
    }
}