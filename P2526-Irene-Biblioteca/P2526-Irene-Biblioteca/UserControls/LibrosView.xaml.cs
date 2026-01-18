using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class LibrosView : UserControl
    {
        private LibrosViewModel VM => (LibrosViewModel)DataContext;

        public LibrosView()
        {
            InitializeComponent();
            DataContext = new LibrosViewModel();
        }

        private void LibroSelect_Click(object sender, RoutedEventArgs e)
        {
            var libro = (sender as FrameworkElement)?.DataContext as Libro;
            if (libro == null) return;

            VM.LibroSeleccionado = libro;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            VM.Add();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            VM.Update();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (VM.LibroSeleccionado == null)
            {
                VM.ErrorText = "Selecciona un libro.";
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este libro?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            VM.Delete();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            VM.Limpiar();
        }
    }
}
