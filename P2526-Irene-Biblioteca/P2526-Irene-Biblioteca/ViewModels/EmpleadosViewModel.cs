using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class EmpleadosViewModel : BaseViewModel
    {
        public bool EsEmpleado => SesionActual.EsEmpleado;

        public bool EsSoloLectura => !SesionActual.EsEmpleado;

        private readonly EmpleadosRepository repo = new EmpleadosRepository();

        public ObservableCollection<Empleado> Empleados { get; set; }

        private Empleado empleadoSeleccionado;
        public Empleado EmpleadoSeleccionado
        {
            get => empleadoSeleccionado;
            set { empleadoSeleccionado = value; OnPropertyChanged(); }
        }

        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public EmpleadosViewModel()
        {
            Cargar();

            AddCommand = new RelayCommand(_ => Add());
            UpdateCommand = new RelayCommand(_ => Update(), _ => EmpleadoSeleccionado != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => EmpleadoSeleccionado != null);
            ClearCommand = new RelayCommand(_ => Limpiar());
        }

        private void Cargar()
        {
            Empleados = new ObservableCollection<Empleado>(repo.GetAll());
            OnPropertyChanged(nameof(Empleados));
        }

        private void Add()
        {
            repo.Insert(new Empleado
            {
                Nombre = Nombre,
                Usuario = Usuario
            }, Password);

            Limpiar();
            Cargar();
        }

        private void Update()
        {
            repo.Update(EmpleadoSeleccionado);

            if (!string.IsNullOrWhiteSpace(Password))
                repo.UpdatePassword(EmpleadoSeleccionado.IdEmpleado, Password);

            Limpiar();
            Cargar();
        }

        private void Delete()
        {
            repo.Delete(EmpleadoSeleccionado.IdEmpleado);
            Limpiar();
            Cargar();
        }

        private void Limpiar()
        {
            Nombre = Usuario = Password = null;
            EmpleadoSeleccionado = null;

            OnPropertyChanged(nameof(Nombre));
            OnPropertyChanged(nameof(Usuario));
            OnPropertyChanged(nameof(Password));
        }
    }
}
