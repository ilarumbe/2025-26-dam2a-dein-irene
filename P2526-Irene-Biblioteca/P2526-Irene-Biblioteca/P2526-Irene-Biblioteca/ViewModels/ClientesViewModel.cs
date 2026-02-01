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

        private string mensaje;
        public string Mensaje
        {
            get => mensaje;
            set { mensaje = value; OnPropertyChanged(); }
        }

        public bool PuedeEditar => service.PuedeEditar();

        public ClientesViewModel()
        {
            Cargar();
            Mensaje = "Selecciona un cliente o crea uno nuevo.";
        }

        public void Cargar()
        {
            Clientes.Clear();
            foreach (var c in repo.GetAll())
                Clientes.Add(c);
        }

        private void CargarDesdeSeleccion()
        {
            if (ClienteSeleccionado == null) return;

            Nombre = ClienteSeleccionado.Nombre;
            Usuario = ClienteSeleccionado.Usuario;
            Password = "";
            Mensaje = "Editando cliente seleccionado.";
        }

        public void Limpiar()
        {
            ClienteSeleccionado = null;
            Nombre = "";
            Usuario = "";
            Password = "";
        }

        public void Add()
        {
            if (!service.ValidarAlta(Nombre, Usuario, Password, out string error))
            {
                Mensaje = error;
                return;
            }

            repo.Insert(new Cliente
            {
                Nombre = Nombre.Trim(),
                Usuario = Usuario.Trim()
            }, Password);

            Mensaje = "Cliente añadido correctamente.";
            Limpiar();
            Cargar();
        }

        public void Update()
        {
            if (ClienteSeleccionado == null)
            {
                Mensaje = "Selecciona un cliente.";
                return;
            }

            int? id = ClienteSeleccionado.IdCliente;
            if (!service.ValidarModificacion(id, Nombre, Usuario, out string error))
            {
                Mensaje = error;
                return;
            }

            ClienteSeleccionado.Nombre = Nombre.Trim();
            ClienteSeleccionado.Usuario = Usuario.Trim();

            repo.Update(ClienteSeleccionado);

            if (!string.IsNullOrWhiteSpace(Password))
                repo.UpdatePassword(ClienteSeleccionado.IdCliente, Password);

            Mensaje = "Cliente modificado correctamente.";
            Limpiar();
            Cargar();
        }

        public void Delete()
        {
            if (ClienteSeleccionado == null)
            {
                Mensaje = "Selecciona un cliente.";
                return;
            }

            int? id = ClienteSeleccionado.IdCliente;
            if (!service.ValidarBorrado(id, out string error))
            {
                Mensaje = error;
                return;
            }

            repo.Delete(ClienteSeleccionado.IdCliente);

            Mensaje = "Cliente eliminado correctamente.";
            Limpiar();
            Cargar();
        }
    }
}
