using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Configuration;

namespace _1._13_Irene_ArtWorld
{
    public partial class CuadroDetalles : Window
    {
        private int idCuadro;
        private string connectionString;

        public CuadroDetalles(int id)
        {
            InitializeComponent();
            idCuadro = id;
            connectionString = ConfigurationManager.ConnectionStrings["Conexion"]?.ConnectionString;
            CargarCuadro();
        }

        private void CargarCuadro()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Nombre, Descripcion, Imagen FROM Cuadros WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idCuadro);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtNombre.Text = reader["Nombre"].ToString();
                    txtDescripcion.Text = reader["Descripcion"].ToString();

                    string rutaImagen = reader["Imagen"].ToString();
                    imgCuadro.Source = new BitmapImage(new Uri(rutaImagen, UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
}
