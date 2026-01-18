using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.ObjectModel;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class ClientesViewModel : BaseViewModel
    {
        private readonly ClientesRepository repo = new ClientesRepository();
        private readonly ClientesService service = new ClientesService();

        public ObservableCollection<Cliente> Clientes { get; } = new ObservableCollection<Cliente>();

        private Cliente clienteSeleccionado;
        public Cliente ClienteSeleccionado
        {
            get => clienteSeleccionado;
            set
            {
                clienteSeleccionado = value;
                OnPropertyChanged();
                CargarDesdeSeleccion();
            }
        }

        private string nombre;
        public string Nombre
        {
            get => nombre;
            set { nombre = value; OnPropertyChanged(); }
        }

        private string usuario;
        public string Usuario
        {
            get => usuario;
            set { usuario = value; OnPropertyChanged(); }
        }

        private string password;
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

        private string errorText;
        public string ErrorText
        {
            get => errorText;
            set { errorText = value; OnPropertyChanged(); }
        }

        public bool PuedeEditar => service.PuedeEditar();

        public ClientesViewModel()
        {
            Cargar();
        }

        public void Cargar()
        {
            Clientes.Clear();
            foreach (var c in repo.GetAll())
                Clientes.Add(c);

            ErrorText = "";
        }

        private void CargarDesdeSeleccion()
        {
            if (ClienteSeleccionado == null) return;

            Nombre = ClienteSeleccionado.Nombre;
            Usuario = ClienteSeleccionado.Usuario;
            Password = "";
            ErrorText = "";
        }

        public void Limpiar()
        {
            ClienteSeleccionado = null;
            Nombre = "";
            Usuario = "";
            Password = "";
            ErrorText = "";
        }

        public void Add()
        {
            ErrorText = "";

            if (!service.ValidarAlta(Nombre, Usuario, Password, out string error))
            {
                ErrorText = error;
                return;
            }

            repo.Insert(new Cliente
            {
                Nombre = Nombre.Trim(),
                Usuario = Usuario.Trim()
            }, Password);

            Limpiar();
            Cargar();
        }

        public void Update()
        {
            ErrorText = "";

            int? id = ClienteSeleccionado?.IdCliente;
            if (!service.ValidarModificacion(id, Nombre, Usuario, out string error))
            {
                ErrorText = error;
                return;
            }

            ClienteSeleccionado.Nombre = Nombre.Trim();
            ClienteSeleccionado.Usuario = Usuario.Trim();

            repo.Update(ClienteSeleccionado);

            if (!string.IsNullOrWhiteSpace(Password))
                repo.UpdatePassword(ClienteSeleccionado.IdCliente, Password);

            Limpiar();
            Cargar();
        }

        public void Delete()
        {
            ErrorText = "";

            int? id = ClienteSeleccionado?.IdCliente;
            if (!service.ValidarBorrado(id, out string error))
            {
                ErrorText = error;
                return;
            }

            repo.Delete(ClienteSeleccionado.IdCliente);

            Limpiar();
            Cargar();
        }
    }
}
