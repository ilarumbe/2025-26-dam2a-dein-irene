using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.ObjectModel;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class EmpleadosViewModel : BaseViewModel
    {
        private readonly EmpleadosRepository repo = new EmpleadosRepository();
        private readonly EmpleadosService service = new EmpleadosService();

        public ObservableCollection<Empleado> Empleados { get; } = new ObservableCollection<Empleado>();

        private Empleado empleadoSeleccionado;
        public Empleado EmpleadoSeleccionado
        {
            get => empleadoSeleccionado;
            set
            {
                empleadoSeleccionado = value;
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

        public EmpleadosViewModel()
        {
            Cargar();
            Mensaje = "Selecciona un empleado o crea uno nuevo.";
        }

        public void Cargar()
        {
            Empleados.Clear();
            foreach (var e in repo.GetAll())
                Empleados.Add(e);
        }

        private void CargarDesdeSeleccion()
        {
            if (EmpleadoSeleccionado == null) return;

            Nombre = EmpleadoSeleccionado.Nombre;
            Usuario = EmpleadoSeleccionado.Usuario;
            Password = "";
            Mensaje = "Editando empleado seleccionado.";
        }

        public void Limpiar()
        {
            EmpleadoSeleccionado = null;
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

            repo.Insert(new Empleado
            {
                Nombre = Nombre.Trim(),
                Usuario = Usuario.Trim()
            }, Password);

            Mensaje = "Empleado añadido correctamente.";
            Limpiar();
            Cargar();
        }

        public void Update()
        {
            if (EmpleadoSeleccionado == null)
            {
                Mensaje = "Selecciona un empleado.";
                return;
            }

            int? id = EmpleadoSeleccionado.IdEmpleado;
            if (!service.ValidarModificacion(Nombre, Usuario, id, out string error))
            {
                Mensaje = error;
                return;
            }

            EmpleadoSeleccionado.Nombre = Nombre.Trim();
            EmpleadoSeleccionado.Usuario = Usuario.Trim();

            repo.Update(EmpleadoSeleccionado);

            if (!string.IsNullOrWhiteSpace(Password))
                repo.UpdatePassword(EmpleadoSeleccionado.IdEmpleado, Password);

            Mensaje = "Empleado modificado correctamente.";
            Limpiar();
            Cargar();
        }

        public void Delete()
        {
            if (EmpleadoSeleccionado == null)
            {
                Mensaje = "Selecciona un empleado.";
                return;
            }

            int? id = EmpleadoSeleccionado.IdEmpleado;
            if (!service.ValidarBorrado(id, out string error))
            {
                Mensaje = error;
                return;
            }

            repo.Delete(EmpleadoSeleccionado.IdEmpleado);

            Mensaje = "Empleado eliminado correctamente.";
            Limpiar();
            Cargar();
        }
    }
}
