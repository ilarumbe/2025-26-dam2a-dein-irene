using System;
using System.Collections.Generic;
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

namespace A1._11_Orla_Irene
{
    public partial class Puesto : UserControl
    {

        public static DependencyProperty NombreProperty = DependencyProperty.Register("Nombre", typeof(string), typeof(Puesto), new PropertyMetadata(string.Empty));
        public static DependencyProperty ApellidosProperty = DependencyProperty.Register("Apellidos", typeof(string), typeof(Puesto), new PropertyMetadata(string.Empty));
        public static DependencyProperty EmailProperty = DependencyProperty.Register("Email", typeof(string), typeof(Puesto), new PropertyMetadata(string.Empty));
        public static DependencyProperty FotoProperty = DependencyProperty.Register("Foto", typeof(string), typeof(Puesto), new PropertyMetadata(string.Empty));

        public Puesto()
        {
            InitializeComponent();
        }

        public string Nombre
        {
            get { return (string)GetValue(NombreProperty); }
            set { SetValue(NombreProperty, value); }
        }

        public string Apellidos
        {
            get { return (string)GetValue(ApellidosProperty); }
            set { SetValue(ApellidosProperty, value); }
        }

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        public string Foto
        {
            get { return (string)GetValue(FotoProperty); }
            set { SetValue(FotoProperty, value); }
        }


        private void Persona_MouseEnter(object sender, MouseEventArgs e)
        {
            LabelPuesto.Text = Nombre + " " + Apellidos;
        }

        private void Persona_MouseLeave(object sender, MouseEventArgs e)
        {
            LabelPuesto.Text = "";
        }

        private void Persona_Click(object sender, RoutedEventArgs e)
        {
            LabelPuesto.Text = Email;
        }

        private void Persona_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Profile win2 = new Profile(Nombre, Apellidos, Email, Foto);
            win2.Show();
        }
    }
}
