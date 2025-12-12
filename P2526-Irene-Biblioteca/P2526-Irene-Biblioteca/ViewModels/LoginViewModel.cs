using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using P2526_Irene_Biblioteca.Views;
using System.Windows.Input;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IWindowService windowService;

        public string Usuario { get; set; }
        public string Password { get; set; }

        private string mensajeError;
        public string MensajeError
        {
            get => mensajeError;
            set { mensajeError = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IWindowService service)
        {
            windowService = service;
            LoginCommand = new RelayCommand(o => Login());
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                MensajeError = "Introduce usuario y contraseña";
                return;
            }

            var empRepo = new EmpleadosRepository();
            var empleado = empRepo.GetByUsuarioAndPassword(Usuario, Password);

            if (empleado != null)
            {
                AbrirMainWindow();
                return;
            }

            var cliRepo = new ClientesRepository();
            var cliente = cliRepo.GetByUsuarioAndPassword(Usuario, Password);

            if (cliente != null)
            {
                AbrirMainWindow();
                return;
            }

            MensajeError = "Usuario o contraseña incorrectos";
        }

        private void AbrirMainWindow()
        {
            var main = new MainWindow();
            main.Show();

            windowService.Close();
        }
    }
}
