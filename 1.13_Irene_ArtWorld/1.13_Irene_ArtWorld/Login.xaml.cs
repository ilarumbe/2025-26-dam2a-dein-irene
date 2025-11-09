using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using WPF___BBDD_Usuarios.Helpers;

namespace _1._13_Irene_ArtWorld
{
    public partial class LoginWindow : Window
    {
        private string connectionString;

        public LoginWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["Conexion"]?.ConnectionString;
        }

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Password.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Por favor, completa ambos campos.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashLocal = Helper.HashPassword(contrasena);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Contrasena FROM Usuarios WHERE Usuario=@Usuario";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Usuario", usuario);

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        Error ventanaError = new Error();
                        ventanaError.Show();
                        this.Close();
                        return;
                    }

                    string hashBD = result.ToString().Trim()
                        .Replace("\r", "")
                        .Replace("\n", "");

                    hashLocal = hashLocal.Trim()
                        .Replace("\r", "")
                        .Replace("\n", "");

                    if (string.Equals(hashLocal, hashBD, StringComparison.Ordinal))
                    {
                        Menu menu = new Menu();
                        menu.Show();
                        this.Close();
                    }
                    else
                    {
                        Error ventanaError = new Error();
                        ventanaError.Show();
                        this.Close();
                    }
                }
            }
            catch (Exception)
            {
                Error ventanaError = new Error();
                ventanaError.Show();
                this.Close();
            }
        }

    }
}
