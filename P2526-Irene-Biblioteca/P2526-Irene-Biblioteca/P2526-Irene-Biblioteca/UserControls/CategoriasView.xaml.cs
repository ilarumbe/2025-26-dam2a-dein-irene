using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class CategoriasView : UserControl
    {
        private CategoriasViewModel VM => (CategoriasViewModel)DataContext;

        public CategoriasView()
        {
            InitializeComponent();
            DataContext = new CategoriasViewModel();
        }

        private void CategoriaSelect_Click(object sender, RoutedEventArgs e)
        {
            var cat = (sender as FrameworkElement)?.DataContext as Categoria;
            if (cat == null) return;

            VM.CategoriaSeleccionada = cat;
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
            if (VM.CategoriaSeleccionada == null)
            {
                VM.Mensaje = "Selecciona una categoría.";
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar esta categoría?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            VM.Delete();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            VM.Limpiar();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Focus();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!VM.PuedeEditar)
                return;

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                VM.Add();
                e.Handled = true;
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                VM.Update();
                e.Handled = true;
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D)
            {
                VM.Delete();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                VM.Limpiar();
                e.Handled = true;
            }
        }
    }
}
