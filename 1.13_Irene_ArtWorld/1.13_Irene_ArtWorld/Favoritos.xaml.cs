using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Configuration;

namespace _1._13_Irene_ArtWorld
{
    public partial class Favoritos : Window
    {
        private readonly string connectionString;

        public Favoritos()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Conexion"]?.ConnectionString;
            CargarFavoritos();
        }

        private void CargarFavoritos()
        {
            List<Cuadro> favoritos = new List<Cuadro>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Nombre, Imagen FROM Cuadros WHERE Nombre IN ('La noche estrellada', 'El jardín de las delicias')";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    favoritos.Add(new Cuadro
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Imagen = reader["Imagen"].ToString()
                    });
                }
            }

            PanelFavoritos.ItemsSource = favoritos;
        }

        private void Cuadro_Click(object sender, MouseButtonEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is Cuadro cuadro)
            {
                CuadroDetalles detalles = new CuadroDetalles(cuadro.Id);
                detalles.Show();
                this.Close();
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