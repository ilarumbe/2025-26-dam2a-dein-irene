using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System;
using System.Collections.ObjectModel;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class PrestamosViewModel : BaseViewModel
    {
        private readonly PrestamosRepository repoPrestamos = new PrestamosRepository();
        private readonly LibrosRepository repoLibros = new LibrosRepository();
        private readonly ClientesRepository repoClientes = new ClientesRepository();
        private readonly EmpleadosRepository repoEmpleados = new EmpleadosRepository();
        private readonly PrestamosService service = new PrestamosService();

        public ObservableCollection<Prestamo> Prestamos { get; } = new ObservableCollection<Prestamo>();
        public ObservableCollection<Libro> Libros { get; } = new ObservableCollection<Libro>();
        public ObservableCollection<Cliente> Clientes { get; } = new ObservableCollection<Cliente>();
        public ObservableCollection<Empleado> Empleados { get; } = new ObservableCollection<Empleado>();

        private Prestamo prestamoSeleccionado;
        public Prestamo PrestamoSeleccionado
        {
            get => prestamoSeleccionado;
            set
            {
                prestamoSeleccionado = value;
                OnPropertyChanged();
                CargarDesdeSeleccion();
            }
        }

        private int? libroId;
        public int? LibroId
        {
            get => libroId;
            set { libroId = value; OnPropertyChanged(); }
        }

        private int? clienteId;
        public int? ClienteId
        {
            get => clienteId;
            set { clienteId = value; OnPropertyChanged(); }
        }

        private int? empleadoId;
        public int? EmpleadoId
        {
            get => empleadoId;
            set { empleadoId = value; OnPropertyChanged(); }
        }

        private DateTime? fechaPrestamo;
        public DateTime? FechaPrestamo
        {
            get => fechaPrestamo;
            set { fechaPrestamo = value; OnPropertyChanged(); }
        }

        private DateTime? fechaDevolucion;
        public DateTime? FechaDevolucion
        {
            get => fechaDevolucion;
            set { fechaDevolucion = value; OnPropertyChanged(); }
        }

        private bool devuelto;
        public bool Devuelto
        {
            get => devuelto;
            set { devuelto = value; OnPropertyChanged(); }
        }

        private string errorText;
        public string ErrorText
        {
            get => errorText;
            set { errorText = value; OnPropertyChanged(); }
        }

        public bool PuedeEditar => service.PuedeEditar();

        public PrestamosViewModel()
        {
            CargarCombos();
            CargarPrestamos();
        }

        public void CargarCombos()
        {
            Libros.Clear();
            foreach (var l in repoLibros.GetAll())
                Libros.Add(l);

            Clientes.Clear();
            foreach (var c in repoClientes.GetAll())
                Clientes.Add(c);

            Empleados.Clear();
            foreach (var e in repoEmpleados.GetAll())
                Empleados.Add(e);
        }

        public void CargarPrestamos()
        {
            Prestamos.Clear();
            foreach (var p in repoPrestamos.GetAll())
                Prestamos.Add(p);

            ErrorText = "";
        }

        private void CargarDesdeSeleccion()
        {
            if (PrestamoSeleccionado == null) return;

            LibroId = PrestamoSeleccionado.IdLibro;
            ClienteId = PrestamoSeleccionado.IdCliente;
            EmpleadoId = PrestamoSeleccionado.IdEmpleado;

            FechaPrestamo = PrestamoSeleccionado.FechaPrestamo;
            FechaDevolucion = PrestamoSeleccionado.FechaDevolucion;

            Devuelto = PrestamoSeleccionado.Devuelto;
            ErrorText = "";
        }

        public void Limpiar()
        {
            PrestamoSeleccionado = null;

            LibroId = null;
            ClienteId = null;
            EmpleadoId = null;

            FechaPrestamo = null;
            FechaDevolucion = null;

            Devuelto = false;
            ErrorText = "";
        }

        private object GetLibroObj()
        {
            if (!LibroId.HasValue) return null;
            foreach (var l in Libros)
                if (l.IdLibro == LibroId.Value) return l;
            return null;
        }

        private object GetClienteObj()
        {
            if (!ClienteId.HasValue) return null;
            foreach (var c in Clientes)
                if (c.IdCliente == ClienteId.Value) return c;
            return null;
        }

        private object GetEmpleadoObj()
        {
            if (!EmpleadoId.HasValue) return null;
            foreach (var e in Empleados)
                if (e.IdEmpleado == EmpleadoId.Value) return e;
            return null;
        }

        public void Add()
        {
            ErrorText = "";

            DateTime fp = FechaPrestamo ?? default(DateTime);
            if (!service.ValidarAlta(GetLibroObj(), GetClienteObj(), GetEmpleadoObj(), fp, out string error))
            {
                ErrorText = error;
                return;
            }

            int idLibro = LibroId.Value;

            if (!service.PuedePrestar(idLibro, out error))
            {
                ErrorText = error;
                return;
            }

            repoPrestamos.Insert(new Prestamo
            {
                IdLibro = idLibro,
                IdCliente = ClienteId.Value,
                IdEmpleado = EmpleadoId.Value,
                FechaPrestamo = fp,
                FechaDevolucion = FechaDevolucion,
                Devuelto = Devuelto
            });

            if (!Devuelto)
                service.AplicarPrestamoStock(idLibro);

            Limpiar();
            CargarPrestamos();
            CargarCombos();
        }

        public void Update()
        {
            ErrorText = "";

            if (PrestamoSeleccionado == null)
            {
                ErrorText = "Selecciona un préstamo.";
                return;
            }

            int oldLibro = PrestamoSeleccionado.IdLibro;
            bool oldDevuelto = PrestamoSeleccionado.Devuelto;

            int newLibro = LibroId.Value;
            bool newDevuelto = Devuelto;

            PrestamoSeleccionado.IdLibro = newLibro;
            PrestamoSeleccionado.IdCliente = ClienteId.Value;
            PrestamoSeleccionado.IdEmpleado = EmpleadoId.Value;
            PrestamoSeleccionado.FechaPrestamo = FechaPrestamo ?? PrestamoSeleccionado.FechaPrestamo;
            PrestamoSeleccionado.FechaDevolucion = FechaDevolucion;
            PrestamoSeleccionado.Devuelto = newDevuelto;

            if (!oldDevuelto && newDevuelto)
                service.AplicarDevolucionStock(oldLibro);

            if (oldDevuelto && !newDevuelto)
            {
                if (!service.PuedePrestar(newLibro, out string err))
                {
                    ErrorText = err;
                    return;
                }
                service.AplicarPrestamoStock(newLibro);
            }

            if (!newDevuelto && oldLibro != newLibro)
            {
                service.AplicarDevolucionStock(oldLibro);

                if (!service.PuedePrestar(newLibro, out string err))
                {
                    service.AplicarPrestamoStock(oldLibro);
                    ErrorText = err;
                    return;
                }
                service.AplicarPrestamoStock(newLibro);
            }

            repoPrestamos.Update(PrestamoSeleccionado);

            Limpiar();
            CargarPrestamos();
            CargarCombos();
        }

        public void Delete()
        {
            ErrorText = "";

            if (PrestamoSeleccionado == null)
            {
                ErrorText = "Selecciona un préstamo.";
                return;
            }

            if (!PrestamoSeleccionado.Devuelto)
                service.AplicarDevolucionStock(PrestamoSeleccionado.IdLibro);

            repoPrestamos.Delete(PrestamoSeleccionado.IdPrestamo);

            Limpiar();
            CargarPrestamos();
            CargarCombos();
        }
    }
}
