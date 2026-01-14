using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class EmpleadosView : UserControl
    {
        private readonly EmpleadosRepository repo = new EmpleadosRepository();
        private readonly EmpleadosService service = new EmpleadosService();

        private Empleado empleadoSeleccionado;

        public EmpleadosView()
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
            List<Empleado> lista = repo.GetAll();
            EmpleadosGrid.ItemsSource = lista;
        }

        private void EmpleadosGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            empleadoSeleccionado = EmpleadosGrid.SelectedItem as Empleado;

            if (empleadoSeleccionado == null)
                return;

            NombreTextBox.Text = empleadoSeleccionado.Nombre;
            UsuarioTextBox.Text = empleadoSeleccionado.Usuario;

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

            repo.Insert(new Empleado
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

            int? id = empleadoSeleccionado?.IdEmpleado;
            if (!service.ValidarModificacion(NombreTextBox.Text, UsuarioTextBox.Text, id, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            empleadoSeleccionado.Nombre = NombreTextBox.Text.Trim();
            empleadoSeleccionado.Usuario = UsuarioTextBox.Text.Trim();

            repo.Update(empleadoSeleccionado);

            if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
                repo.UpdatePassword(empleadoSeleccionado.IdEmpleado, PasswordBox.Password);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = empleadoSeleccionado?.IdEmpleado;
            if (!service.ValidarBorrado(id, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este empleado?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            repo.Delete(empleadoSeleccionado.IdEmpleado);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            empleadoSeleccionado = null;
            EmpleadosGrid.SelectedItem = null;

            NombreTextBox.Clear();
            UsuarioTextBox.Clear();
            PasswordBox.Clear();
            ErrorText.Text = "";
        }
    }
}
