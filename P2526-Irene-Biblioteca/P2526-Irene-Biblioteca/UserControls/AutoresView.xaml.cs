using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class AutoresView : UserControl
    {
        private readonly AutoresRepository repo = new AutoresRepository();
        private readonly AutoresService service = new AutoresService();

        private Autor autorSeleccionado;

        public AutoresView()
        {
            InitializeComponent();
            Cargar();
            AplicarPermisos();
        }

        private void AplicarPermisos()
        {
            bool puede = service.PuedeEditar();

            NombreTextBox.IsEnabled = puede;
            NacionalidadTextBox.IsEnabled = puede;
        }

        private void Cargar()
        {
            List<Autor> lista = repo.GetAll();
            AutoresGrid.ItemsSource = lista;
            AutoresCards.ItemsSource = lista;
        }

        private void AutoresGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            autorSeleccionado = AutoresGrid.SelectedItem as Autor;

            if (autorSeleccionado == null)
                return;

            NombreTextBox.Text = autorSeleccionado.Nombre;
            NacionalidadTextBox.Text = autorSeleccionado.Nacionalidad;
            ErrorText.Text = "";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            if (!service.ValidarAlta(NombreTextBox.Text, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            repo.Insert(new Autor
            {
                Nombre = NombreTextBox.Text.Trim(),
                Nacionalidad = string.IsNullOrWhiteSpace(NacionalidadTextBox.Text)
                    ? null
                    : NacionalidadTextBox.Text.Trim()
            });

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = autorSeleccionado?.IdAutor;
            if (!service.ValidarModificacion(id, NombreTextBox.Text, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            autorSeleccionado.Nombre = NombreTextBox.Text.Trim();
            autorSeleccionado.Nacionalidad = string.IsNullOrWhiteSpace(NacionalidadTextBox.Text)
                ? null
                : NacionalidadTextBox.Text.Trim();

            repo.Update(autorSeleccionado);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = autorSeleccionado?.IdAutor;
            if (!service.ValidarBorrado(id, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este autor?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            repo.Delete(autorSeleccionado.IdAutor);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            autorSeleccionado = null;
            AutoresGrid.SelectedItem = null;

            NombreTextBox.Clear();
            NacionalidadTextBox.Clear();
            ErrorText.Text = "";
        }

        private void AutorSelect_Click(object sender, RoutedEventArgs e)
        {
            var autor = (sender as FrameworkElement)?.DataContext as Autor;
            if (autor == null) return;

            autorSeleccionado = autor;

            NombreTextBox.Text = autorSeleccionado.Nombre;
            NacionalidadTextBox.Text = autorSeleccionado.Nacionalidad;
            ErrorText.Text = "";
        }

    }
}
