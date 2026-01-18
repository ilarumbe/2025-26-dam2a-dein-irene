using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;

namespace P2526_Irene_Biblioteca.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel VM => (MainWindowViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void Autores_Click(object sender, RoutedEventArgs e) => VM.ShowAutores();
        private void Libros_Click(object sender, RoutedEventArgs e) => VM.ShowLibros();
        private void Categorias_Click(object sender, RoutedEventArgs e) => VM.ShowCategorias();
        private void Clientes_Click(object sender, RoutedEventArgs e) => VM.ShowClientes();
        private void Empleados_Click(object sender, RoutedEventArgs e) => VM.ShowEmpleados();
        private void Prestamos_Click(object sender, RoutedEventArgs e) => VM.ShowPrestamos();

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
