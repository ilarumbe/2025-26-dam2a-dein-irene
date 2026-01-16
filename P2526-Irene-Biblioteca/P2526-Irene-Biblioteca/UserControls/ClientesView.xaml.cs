using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class ClientesView : UserControl
    {
        private readonly ClientesRepository repo = new ClientesRepository();
        private readonly ClientesService service = new ClientesService();

        private Cliente clienteSeleccionado;

        public ClientesView()
        {
            InitializeComponent();
            Cargar();
            AplicarPermisos();
        }

        private void AplicarPermisos()
        {
            bool puede = service.PuedeEditar();

            NombreTextBox.IsEnabled = puede;
            UsuarioTextBox.IsEnabled = puede;
            PasswordBox.IsEnabled = puede;
        }

        private void Cargar()
        {
            List<Cliente> lista = repo.GetAll();
            ClientesGrid.ItemsSource = lista;
        }

        private void ClientesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clienteSeleccionado = ClientesGrid.SelectedItem as Cliente;

            if (clienteSeleccionado == null)
                return;

            NombreTextBox.Text = clienteSeleccionado.Nombre;
            UsuarioTextBox.Text = clienteSeleccionado.Usuario;
            PasswordBox.Clear();
            ErrorText.Text = "";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            if (!service.ValidarAlta(NombreTextBox.Text, UsuarioTextBox.Text, PasswordBox.Password, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            repo.Insert(new Cliente
            {
                Nombre = NombreTextBox.Text.Trim(),
                Usuario = UsuarioTextBox.Text.Trim()
            }, PasswordBox.Password);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = clienteSeleccionado?.IdCliente;
            if (!service.ValidarModificacion(id, NombreTextBox.Text, UsuarioTextBox.Text, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            clienteSeleccionado.Nombre = NombreTextBox.Text.Trim();
            clienteSeleccionado.Usuario = UsuarioTextBox.Text.Trim();

            repo.Update(clienteSeleccionado);

            if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
                repo.UpdatePassword(clienteSeleccionado.IdCliente, PasswordBox.Password);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = clienteSeleccionado?.IdCliente;
            if (!service.ValidarBorrado(id, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este cliente?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            repo.Delete(clienteSeleccionado.IdCliente);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            clienteSeleccionado = null;
            ClientesGrid.SelectedItem = null;

            NombreTextBox.Clear();
            UsuarioTextBox.Clear();
            PasswordBox.Clear();
            ErrorText.Text = "";
        }
    }
}
