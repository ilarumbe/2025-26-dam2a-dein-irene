using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                PasswordBox.Clear();
                e.Handled = true;
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                VM.Update();
                PasswordBox.Clear();
                e.Handled = true;
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D)
            {
                VM.Delete();
                PasswordBox.Clear();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                VM.Limpiar();
                PasswordBox.Clear();
                e.Handled = true;
            }
        }
    }
}
