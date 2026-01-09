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

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class EmpleadosView : UserControl
    {
        public EmpleadosView()
        {
            InitializeComponent();
            Loaded += EmpleadosView_Loaded;
        }
        private void EmpleadosView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is EmpleadosViewModel vm)
            {
                vm.RequestClearPassword -= Vm_RequestClearPassword;
                vm.RequestClearPassword += Vm_RequestClearPassword;
            }
        }

        private void Vm_RequestClearPassword()
        {
            PasswordBox.Clear();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is EmpleadosViewModel vm)
                vm.Password = PasswordBox.Password;
        }
    }
}
