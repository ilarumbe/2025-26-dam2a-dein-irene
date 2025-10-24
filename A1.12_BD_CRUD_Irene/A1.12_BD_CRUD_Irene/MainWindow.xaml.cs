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
        private int selectedId = -1;
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Videojuegos (Titulo, Genero, Plataforma, Desarrollador, FechaLanzamiento, Precio) " +
                                   "VALUES (@Titulo, @Genero, @Plataforma, @Desarrollador, @FechaLanzamiento, @Precio)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("@Genero", txtGenero.Text);
                        cmd.Parameters.AddWithValue("@Plataforma", txtPlataforma.Text);
                        cmd.Parameters.AddWithValue("@Desarrollador", txtDesarrollador.Text);
                        cmd.Parameters.AddWithValue("@FechaLanzamiento", dpFechaLanzamiento.SelectedDate.Value);
                        cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Videojuego guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarVideojuegos(string filtro = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Videojuegos";
                    if (!string.IsNullOrWhiteSpace(filtro))
                        query += " WHERE Titulo LIKE @Filtro";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    if (!string.IsNullOrWhiteSpace(filtro))
                        da.SelectCommand.Parameters.AddWithValue("@Filtro", "%" + filtro + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgVideojuegos.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar videojuegos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Selecciona un videojuego para actualizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Videojuegos SET Titulo=@Titulo, Genero=@Genero, Plataforma=@Plataforma, " +
                                   "Desarrollador=@Desarrollador, FechaLanzamiento=@FechaLanzamiento, Precio=@Precio " +
                                   "WHERE Id=@Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text);
                        cmd.Parameters.AddWithValue("@Genero", txtGenero.Text);
                        cmd.Parameters.AddWithValue("@Plataforma", txtPlataforma.Text);
                        cmd.Parameters.AddWithValue("@Desarrollador", txtDesarrollador.Text);
                        cmd.Parameters.AddWithValue("@FechaLanzamiento", dpFechaLanzamiento.SelectedDate.Value);
                        cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                        cmd.Parameters.AddWithValue("@Id", selectedId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Videojuego actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarVideojuegos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Selecciona un videojuego para eliminar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("¿Deseas eliminar este videojuego?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Videojuegos WHERE Id=@Id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", selectedId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Videojuego eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    CargarVideojuegos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TxtBuscar_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CargarVideojuegos(txtBuscar.Text);
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) ||
                string.IsNullOrWhiteSpace(txtGenero.Text) ||
                string.IsNullOrWhiteSpace(txtPlataforma.Text) ||
                string.IsNullOrWhiteSpace(txtDesarrollador.Text) ||
                !dpFechaLanzamiento.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Completa todos los campos.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!decimal.TryParse(txtPrecio.Text, out _))
            {
                MessageBox.Show("El precio debe ser un número válido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }





    }
}
