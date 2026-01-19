using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class PrestamosView : UserControl
    {
        private PrestamosViewModel VM => (PrestamosViewModel)DataContext;

        public PrestamosView()
        {
            InitializeComponent();
            DataContext = new PrestamosViewModel();
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
            if (VM.PrestamoSeleccionado == null)
            {
                VM.Mensaje = "Selecciona un préstamo.";
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este préstamo?", "Confirmar",
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
