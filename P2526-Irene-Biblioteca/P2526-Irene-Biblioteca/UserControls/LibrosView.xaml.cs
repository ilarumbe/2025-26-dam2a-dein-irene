using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class LibrosView : UserControl
    {
        private readonly LibrosRepository repoLibros = new LibrosRepository();
        private readonly AutoresRepository repoAutores = new AutoresRepository();
        private readonly CategoriasRepository repoCategorias = new CategoriasRepository();
        private readonly LibrosService service = new LibrosService();

        private Libro libroSeleccionado;

        public LibrosView()
        {
            InitializeComponent();
            CargarCombos();
            Cargar();
            AplicarPermisos();
        }

        private void AplicarPermisos()
        {
            bool puede = service.PuedeEditar();

            TituloTextBox.IsEnabled = puede;
            AnioTextBox.IsEnabled = puede;
            AutoresCombo.IsEnabled = puede;
            CategoriasCombo.IsEnabled = puede;
            StockTextBox.IsEnabled = puede;
        }

        private void CargarCombos()
        {
            AutoresCombo.ItemsSource = repoAutores.GetAll();
            CategoriasCombo.ItemsSource = repoCategorias.GetAll();
        }

        private void Cargar()
        {
            List<Libro> lista = repoLibros.GetAll();
            LibrosGrid.ItemsSource = lista;
        }

        private void LibrosGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            libroSeleccionado = LibrosGrid.SelectedItem as Libro;

            if (libroSeleccionado == null)
                return;

            TituloTextBox.Text = libroSeleccionado.Titulo;
            AnioTextBox.Text = libroSeleccionado.Año.ToString();
            StockTextBox.Text = libroSeleccionado.Stock.ToString();

            AutoresCombo.SelectedValue = libroSeleccionado.IdAutor;
            CategoriasCombo.SelectedValue = libroSeleccionado.IdCategoria;

            ErrorText.Text = "";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            if (!service.ValidarAlta(TituloTextBox.Text, AnioTextBox.Text, AutoresCombo.SelectedItem, CategoriasCombo.SelectedItem, StockTextBox.Text, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            repoLibros.Insert(new Libro
            {
                Titulo = TituloTextBox.Text.Trim(),
                Año = int.Parse(AnioTextBox.Text),
                IdAutor = (int)AutoresCombo.SelectedValue,
                IdCategoria = (int)CategoriasCombo.SelectedValue,
                Stock = int.Parse(StockTextBox.Text)
            });

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = libroSeleccionado?.IdLibro;
            if (!service.ValidarModificacion(id, TituloTextBox.Text, AnioTextBox.Text, AutoresCombo.SelectedItem, CategoriasCombo.SelectedItem, StockTextBox.Text, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            libroSeleccionado.Titulo = TituloTextBox.Text.Trim();
            libroSeleccionado.Año = int.Parse(AnioTextBox.Text);
            libroSeleccionado.IdAutor = (int)AutoresCombo.SelectedValue;
            libroSeleccionado.IdCategoria = (int)CategoriasCombo.SelectedValue;
            libroSeleccionado.Stock = int.Parse(StockTextBox.Text);

            repoLibros.Update(libroSeleccionado);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            int? id = libroSeleccionado?.IdLibro;
            if (!service.ValidarBorrado(id, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este libro?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            repoLibros.Delete(libroSeleccionado.IdLibro);

            BtnClear_Click(sender, e);
            Cargar();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            libroSeleccionado = null;
            LibrosGrid.SelectedItem = null;

            TituloTextBox.Clear();
            AnioTextBox.Clear();
            StockTextBox.Clear();

            AutoresCombo.SelectedIndex = -1;
            CategoriasCombo.SelectedIndex = -1;

            ErrorText.Text = "";
        }
    }
}
