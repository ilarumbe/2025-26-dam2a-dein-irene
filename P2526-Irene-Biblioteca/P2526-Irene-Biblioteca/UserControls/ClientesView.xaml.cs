using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class ClientesView : UserControl
    {
        private ClientesViewModel VM => (ClientesViewModel)DataContext;

        public ClientesView()
        {
            InitializeComponent();
            DataContext = new ClientesViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ClientesViewModel vm)
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
            if (VM.ClienteSeleccionado == null)
            {
                VM.Mensaje = "Selecciona un cliente.";
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este cliente?", "Confirmar",
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
