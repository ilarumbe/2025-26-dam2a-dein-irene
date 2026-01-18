using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using P2526_Irene_Biblioteca.Views;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IWindowService windowService;

        private string usuario;
        public string Usuario
        {
            get => usuario;
            set { usuario = value; OnPropertyChanged(); }
        }

        public string Password { get; set; }

        private string mensajeError;
        public string MensajeError
        {
            get => mensajeError;
            set { mensajeError = value; OnPropertyChanged(); }
        }

        public LoginViewModel(IWindowService service)
        {
            windowService = service;
        }

        public void Login()
        {
            MensajeError = "";

            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                MensajeError = "Introduce usuario y contraseña";
                return;
            }

            var empRepo = new EmpleadosRepository();
            var empleado = empRepo.GetByUsuarioAndPassword(Usuario, Password);

            if (empleado != null)
            {
                SesionActual.EsEmpleado = true;
                SesionActual.Usuario = empleado.Usuario;
                AbrirMainWindow();
                return;
            }

            var cliRepo = new ClientesRepository();
            var cliente = cliRepo.GetByUsuarioAndPassword(Usuario, Password);

            if (cliente != null)
            {
                SesionActual.EsEmpleado = false;
                SesionActual.Usuario = cliente.Usuario;
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
