using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class CategoriasView : UserControl
    {
        private readonly CategoriasRepository repo = new CategoriasRepository();
        private readonly CategoriasService service = new CategoriasService();

        private Categoria categoriaSeleccionada;

        public CategoriasView()
        {
            InitializeComponent();
            Cargar();
            AplicarPermisos();
        }

        private void AplicarPermisos()
        {
            bool puede = service.PuedeEditar();

            NombreTextBox.IsEnabled = puede;
            // Si quieres, también puedes deshabilitar/ocultar botones
        }

        private void Cargar()
        {
            List<Categoria> lista = repo.GetAll();
            CategoriasGrid.ItemsSource = lista;
        }

        private void CategoriasGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoriaSeleccionada = CategoriasGrid.SelectedItem as Categoria;

            if (categoriaSeleccionada == null)
                return;

            NombreTextBox.Text = categoriaSeleccionada.Nombre;
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

            repo.Insert(new Categoria
            {
                Nombre = NombreTextBox.Text.Trim()
            });

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = categoriaSeleccionada?.IdCategoria;
            if (!service.ValidarModificacion(id, NombreTextBox.Text, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            categoriaSeleccionada.Nombre = NombreTextBox.Text.Trim();
            repo.Update(categoriaSeleccionada);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = categoriaSeleccionada?.IdCategoria;
            if (!service.ValidarBorrado(id, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar esta categoría?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            repo.Delete(categoriaSeleccionada.IdCategoria);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            categoriaSeleccionada = null;
            CategoriasGrid.SelectedItem = null;

            NombreTextBox.Clear();
            ErrorText.Text = "";
        }
    }
}
