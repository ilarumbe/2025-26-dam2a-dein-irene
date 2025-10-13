using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace A1._10_Formulario_Irene
{
    public partial class MainWindow : Window
    {
        private DataTable dtEmpleados = new DataTable();
        private string rutaFoto;
        public MainWindow()
        {
            InitializeComponent();

            dtEmpleados = new DataTable();
            dtEmpleados.Columns.Add("Nombre");
            dtEmpleados.Columns.Add("Apellidos");
            dtEmpleados.Columns.Add("Email");
            dtEmpleados.Columns.Add("Telefono");

            empleados.ItemsSource = dtEmpleados.DefaultView;
        }
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellidos.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Hay compos obligatorios sin rellenar");
                return;
            }

            DataRow fila = dtEmpleados.NewRow();
            fila["Nombre"] = txtNombre.Text;
            fila["Apellidos"] = txtApellidos.Text;
            fila["Email"] = txtEmail.Text;
            fila["Telefono"] = txtTelefono.Text;

            dtEmpleados.Rows.Add(fila);
            empleados.Items.Refresh();

            txtNombre.Clear();
            txtApellidos.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            txtDNI.Clear();
            dpFechaNacimiento.SelectedDate = null;
            txtDireccion.Text = "Dirección";
            txtCiudad.Text = "Ciudad";
            txtProvincia.Text = "Provincia";
            txtCP.Text = "Código Postal";
            txtPais.Text = "País";
            txtEnlace.Clear();
            cbRol.SelectedIndex = -1;
            txtDescripcion.Clear();
            imgFoto.Source = null;

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow nuevaVentana = new MainWindow();
            nuevaVentana.Show();
            this.Close();
        }
        private void btnCargarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";

            if (dlg.ShowDialog() == true)
            {
                rutaFoto = dlg.FileName;
                BitmapImage imagen = new BitmapImage(new Uri(rutaFoto));
                imgFoto.Source = imagen;
            }
        }

        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "Dirección" || textBox.Text == "Ciudad" || textBox.Text == "Provincia" || textBox.Text == "Código Postal" || textBox.Text == "País")
                {
                    textBox.Text = "";
                }
            }
        }

        private void Txt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox.Name == "txtDireccion")
                        textBox.Text = "Dirección";
                    else if (textBox.Name == "txtCiudad")
                        textBox.Text = "Ciudad";
                    else if (textBox.Name == "txtProvincia")
                        textBox.Text = "Provincia";
                    else if (textBox.Name == "txtCP")
                        textBox.Text = "Código Postal";
                    else if (textBox.Name == "txtPais")
                        textBox.Text = "País";
                }
            }
        }

    }
}