using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _1._13_Irene_ArtWorld
{
    public partial class Menu : Window
    {
        private readonly string connectionString;
        private List<Cuadro> cuadros;

        public Menu()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Conexion"]?.ConnectionString;
            CargarCuadros();
        }

        private void CargarCuadros()
        {
            cuadros = new List<Cuadro>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Cuadros";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cuadros.Add(new Cuadro
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Autor = reader["Autor"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Imagen = reader["Imagen"].ToString()
                    });
                }
            }

            PanelCuadros.ItemsSource = cuadros;
        }

        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cuadros == null || cuadros.Count == 0)
                return;

            string filtro = txtBuscar.Text.ToLower().Trim();

            var filtrados = cuadros
                .Where(c => c.Nombre != null && c.Nombre.ToLower().Contains(filtro))
                .ToList();

            PanelCuadros.ItemsSource = filtrados;
        }

        private void TxtBuscar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtBuscar.Text == "Search")
            {
                txtBuscar.Text = "";
                txtBuscar.Foreground = Brushes.Black;
            }
        }

        private void TxtBuscar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                txtBuscar.Text = "Search";
                txtBuscar.Foreground = Brushes.Gray;
            }
        }

        private void Favoritos_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Favoritos favoritos = new Favoritos();
            favoritos.Show();
            this.Close();
        }

        private void Autores_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Autores autores = new Autores();
            autores.Show();
            this.Close();
        }

        private void Cuadro_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border?.DataContext is Cuadro cuadro)
            {
                CuadroDetalles detalles = new CuadroDetalles(cuadro.Id);
                detalles.Show();
                this.Close();
            }
        }
    }

}