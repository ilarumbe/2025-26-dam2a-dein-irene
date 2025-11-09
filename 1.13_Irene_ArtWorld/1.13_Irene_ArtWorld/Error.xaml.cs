using System.Windows;

namespace _1._13_Irene_ArtWorld
{
    public partial class Error : Window
    {
        public Error()
        {
            InitializeComponent();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}