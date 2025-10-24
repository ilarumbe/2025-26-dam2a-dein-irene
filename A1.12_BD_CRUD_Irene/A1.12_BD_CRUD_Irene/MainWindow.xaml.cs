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






    }
}
