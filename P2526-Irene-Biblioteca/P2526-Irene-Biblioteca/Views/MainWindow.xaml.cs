using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Services;
using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace P2526_Irene_Biblioteca.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel VM => (MainWindowViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
                return;
            }

            try
            {
                DragMove();
            }
            catch { }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaxRestore_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximize();
        }

        private void ToggleMaximize()
        {
            WindowState = (WindowState == WindowState.Maximized)
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                var w = new HelpWindow(VM?.NombreVistaActual);
                w.Owner = this;
                w.ShowDialog();
                e.Handled = true;
            }
        }

        private void Autores_Click(object sender, RoutedEventArgs e)
        {
            if (!SesionActual.EsEmpleado) return;
            VM.ShowAutores();
        }

        private void Libros_Click(object sender, RoutedEventArgs e)
        {
            if (!SesionActual.EsEmpleado) return;
            VM.ShowLibros();
        }

        private void Categorias_Click(object sender, RoutedEventArgs e)
        {
            if (!SesionActual.EsEmpleado) return;
            VM.ShowCategorias();
        }

        private void Clientes_Click(object sender, RoutedEventArgs e)
        {
            if (!SesionActual.EsEmpleado) return;
            VM.ShowClientes();
        }

        private void Empleados_Click(object sender, RoutedEventArgs e)
        {
            if (!SesionActual.EsEmpleado) return;
            VM.ShowEmpleados();
        }

        private void Prestamos_Click(object sender, RoutedEventArgs e)
        {
            VM.ShowPrestamos();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
