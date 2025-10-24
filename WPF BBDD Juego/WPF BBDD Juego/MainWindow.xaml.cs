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

namespace WPF_BBDD_Juego
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString;
        public MainWindow()
        {
            InitializeComponent();
            connectionString =
           ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
            CargarUsuarios();
        }
        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
           string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Por favor, completa todos los campos.",
               "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                using (SqlConnection conn = new
               SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Usuarios (Nombre, Email) VALUES(@Nombre, @Email)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre",
                       txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Email",
                       txtEmail.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Usuario guardado correctamente.", "Éxito",
               MessageBoxButton.OK, MessageBoxImage.Information);
                txtNombre.Clear();
                txtEmail.Clear();
                CargarUsuarios();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CargarUsuarios()
        {
            try
            {
                using (SqlConnection conn = new
               SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Usuarios", conn);
                    
                     DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgUsuarios.ItemsSource = dt.DefaultView;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
