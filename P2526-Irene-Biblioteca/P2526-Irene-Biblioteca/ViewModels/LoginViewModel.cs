using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using P2526_Irene_Biblioteca.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IWindowService windowService;
        private readonly EmpleadosRepository empleadosRepo;

        public string Usuario { get; set; }
        public string Password { get; set; }
        public string MensajeError { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand CloseCommand { get; }

        public LoginViewModel(IWindowService service)
        {
            windowService = service;
            empleadosRepo = new EmpleadosRepository();

            LoginCommand = new RelayCommand(o => Login());
            CloseCommand = new RelayCommand(o => windowService.Close());
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                MensajeError = "Rellena todos los campos.";
                OnPropertyChanged(nameof(MensajeError));
                return;
            }

            string passwordHash = Helper.HashPassword(Password);

            var empleado = empleadosRepo.Login(Usuario, passwordHash);

            if (empleado == null)
            {
                MensajeError = "Usuario o contraseña incorrectos.";
                OnPropertyChanged(nameof(MensajeError));
                return;
            }

            var main = new MainWindow();
            main.Show();

            windowService.Close();
        }
    }
}
