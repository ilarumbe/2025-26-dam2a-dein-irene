using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace A1._12_BD_CRUD_Irene
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString;
        private int idSeleccionado = 0;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Conexion"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
                MessageBox.Show("No se ha leído el connection string");
            else
                CargarVideojuegos();
        }

        private void CargarVideojuegos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Videojuegos", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgVideojuegos.ItemsSource = dt.DefaultView;
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) ||
                string.IsNullOrWhiteSpace(txtGenero.Text) ||
                string.IsNullOrWhiteSpace(txtPlataforma.Text) ||
                string.IsNullOrWhiteSpace(txtDesarrollador.Text) ||
                dpFecha.SelectedDate == null ||
                string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser numérico.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Videojuegos (Titulo, Genero, Plataforma, Desarrollador, FechaLanzamiento, Precio) VALUES (@Titulo, @Genero, @Plataforma, @Desarrollador, @FechaLanzamiento, @Precio)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                    cmd.Parameters.AddWithValue("@Genero", txtGenero.Text);
                    cmd.Parameters.AddWithValue("@Plataforma", txtPlataforma.Text);
                    cmd.Parameters.AddWithValue("@Desarrollador", txtDesarrollador.Text);
                    cmd.Parameters.AddWithValue("@FechaLanzamiento", dpFecha.SelectedDate.Value);
                    cmd.Parameters.AddWithValue("@Precio", precio);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Videojuego guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            LimpiarCampos();
            CargarVideojuegos();
        }

        private void DgVideojuegos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgVideojuegos.SelectedItem is DataRowView fila)
            {
                idSeleccionado = Convert.ToInt32(fila["Id"]);
                txtTitulo.Text = fila["Titulo"].ToString();
                txtGenero.Text = fila["Genero"].ToString();
                txtPlataforma.Text = fila["Plataforma"].ToString();
                txtDesarrollador.Text = fila["Desarrollador"].ToString();
                dpFecha.SelectedDate = Convert.ToDateTime(fila["FechaLanzamiento"]);
                txtPrecio.Text = fila["Precio"].ToString();
            }
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Selecciona un videojuego para actualizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("El precio debe ser numérico.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Videojuegos SET Titulo=@Titulo, Genero=@Genero, Plataforma=@Plataforma, Desarrollador=@Desarrollador, FechaLanzamiento=@FechaLanzamiento, Precio=@Precio WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                    cmd.Parameters.AddWithValue("@Genero", txtGenero.Text);
                    cmd.Parameters.AddWithValue("@Plataforma", txtPlataforma.Text);
                    cmd.Parameters.AddWithValue("@Desarrollador", txtDesarrollador.Text);
                    cmd.Parameters.AddWithValue("@FechaLanzamiento", dpFecha.SelectedDate.Value);
                    cmd.Parameters.AddWithValue("@Precio", precio);
                    cmd.Parameters.AddWithValue("@Id", idSeleccionado);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Videojuego actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            LimpiarCampos();
            CargarVideojuegos();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Selecciona un videojuego para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("¿Seguro que deseas eliminar este videojuego?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Videojuegos WHERE Id=@Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", idSeleccionado);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Videojuego eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarCampos();
                CargarVideojuegos();
            }
        }

        private void TxtBuscar_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Videojuegos WHERE Titulo LIKE @titulo", conn);
                da.SelectCommand.Parameters.AddWithValue("@titulo", "%" + txtBuscar.Text + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgVideojuegos.ItemsSource = dt.DefaultView;
            }
        }

        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtGenero.Clear();
            txtPlataforma.Clear();
            txtDesarrollador.Clear();
            dpFecha.SelectedDate = null;
            txtPrecio.Clear();
            idSeleccionado = 0;
        }
    }

}

