using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class EmpleadosView : UserControl
    {
        private EmpleadosViewModel VM => (EmpleadosViewModel)DataContext;

        public EmpleadosView()
        {
            InitializeComponent();
            DataContext = new EmpleadosViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is EmpleadosViewModel vm)
                vm.Password = PasswordBox.Password;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            VM.Add();
            PasswordBox.Clear();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            VM.Update();
            PasswordBox.Clear();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (VM.EmpleadoSeleccionado == null)
            {
                VM.Mensaje = "Selecciona un empleado.";
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este empleado?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            VM.Delete();
            PasswordBox.Clear();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            VM.Limpiar();
            PasswordBox.Clear();
        }
    }
}
