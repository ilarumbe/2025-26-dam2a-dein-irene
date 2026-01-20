using P2526_Irene_Biblioteca.Repositories;
using System.Windows;

namespace P2526_Irene_Biblioteca.Views
{
    public partial class InformesWindow : Window
    {
        public InformesWindow()
        {
            InitializeComponent();
        }

        private void Autores_Click(object sender, RoutedEventArgs e)
        {
            var repo = new AutoresRepository();
            var datos = repo.GetAll();

            new ReportWindow(
                "Listado de Autores",
                "AutoresListado.rdlc",
                "DSAutor",
                datos
            ).ShowDialog();
        }

        private void Categorias_Click(object sender, RoutedEventArgs e)
        {
            var repo = new CategoriasRepository();
            var datos = repo.GetAll();

            new ReportWindow(
                "Listado de Categorías",
                "CategoriasListado.rdlc",
                "DSCategoria",
                datos
            ).ShowDialog();
        }

        private void Clientes_Click(object sender, RoutedEventArgs e)
        {
            var repo = new ClientesRepository();
            var datos = repo.GetAll();

            new ReportWindow(
                "Listado de Clientes",
                "ClientesListado.rdlc",
                "DSCliente",
                datos
            ).ShowDialog();
        }

        private void Empleados_Click(object sender, RoutedEventArgs e)
        {
            var repo = new EmpleadosRepository();
            var datos = repo.GetAll();

            new ReportWindow(
                "Listado de Empleados",
                "EmpleadosListado.rdlc",
                "DSEmpleado",
                datos
            ).ShowDialog();
        }

        private void Libros_Click(object sender, RoutedEventArgs e)
        {
            var repo = new LibrosRepository();
            var datos = repo.GetAll();

            new ReportWindow(
                "Listado de Libros",
                "LibrosListado.rdlc",
                "DSLibro",
                datos
            ).ShowDialog();
        }

        private void Prestamos_Click(object sender, RoutedEventArgs e)
        {
            var repo = new PrestamosRepository();
            var datos = repo.GetAllReport();

            new ReportWindow(
                "Histórico de Préstamos",
                "PrestamosListado.rdlc",
                "DSPrestamo",
                datos
            ).ShowDialog();
        }
    }
}
