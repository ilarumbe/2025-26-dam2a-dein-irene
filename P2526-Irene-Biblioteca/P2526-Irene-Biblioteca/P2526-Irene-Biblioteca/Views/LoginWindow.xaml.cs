using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Services;
using P2526_Irene_Biblioteca.ViewModels;
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
using System.Windows.Shapes;

namespace P2526_Irene_Biblioteca.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            var service = new WindowService(this);
            DataContext = new LoginViewModel(service);
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).Password = PasswordBox.Password;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Entrar_Click(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).Login();
        }
    }
}
