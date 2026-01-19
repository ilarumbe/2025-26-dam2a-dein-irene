using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

        private int? clienteActualId;

        public bool EsEmpleado => SesionActual.EsEmpleado;

        public bool PuedeEditar
        {
            get
            {
                if (EsEmpleado) return true;
                return clienteActualId.HasValue;
            }
        }

        public bool PuedeElegirCliente => EsEmpleado;
        public bool PuedeElegirEmpleado => EsEmpleado;

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

        private string mensaje;
        public string Mensaje
        {
            get => mensaje;
            set { mensaje = value; OnPropertyChanged(); }
        }

        public PrestamosViewModel()
        {
            if (!EsEmpleado)
            {
                var cli = repoClientes.GetAll().FirstOrDefault(c => c.Usuario == SesionActual.Usuario);
                clienteActualId = cli?.IdCliente;
            }

            CargarCombos();
            CargarPrestamos();

            if (!EsEmpleado)
            {
                ClienteId = clienteActualId;
                FechaPrestamo = DateTime.Today;

                if (Empleados.Count > 0 && !EmpleadoId.HasValue)
                    EmpleadoId = Empleados[0].IdEmpleado;
            }

            Mensaje = "Selecciona un préstamo o crea uno nuevo.";
        }

        public void CargarCombos()
        {
            Libros.Clear();
            foreach (var l in repoLibros.GetAll())
                Libros.Add(l);

            Clientes.Clear();
            var clientesDb = repoClientes.GetAll();

            if (EsEmpleado)
            {
                foreach (var c in clientesDb)
                    Clientes.Add(c);
            }
            else
            {
                var propio = clientesDb.FirstOrDefault(c => c.IdCliente == clienteActualId);
                if (propio != null) Clientes.Add(propio);
            }

            Empleados.Clear();
            foreach (var e in repoEmpleados.GetAll())
                Empleados.Add(e);
        }

        public void CargarPrestamos()
        {
            Prestamos.Clear();
            var lista = repoPrestamos.GetAll();

            if (EsEmpleado)
            {
                foreach (var p in lista)
                    Prestamos.Add(p);
            }
            else
            {
                if (!clienteActualId.HasValue) return;

                foreach (var p in lista.Where(x => x.IdCliente == clienteActualId.Value))
                    Prestamos.Add(p);
            }
        }

        private void CargarDesdeSeleccion()
        {
            if (PrestamoSeleccionado == null) return;

            if (!EsEmpleado && clienteActualId.HasValue && PrestamoSeleccionado.IdCliente != clienteActualId.Value)
            {
                Mensaje = "No tienes permiso para gestionar este préstamo.";
                Limpiar();
                return;
            }

            LibroId = PrestamoSeleccionado.IdLibro;
            ClienteId = PrestamoSeleccionado.IdCliente;
            EmpleadoId = PrestamoSeleccionado.IdEmpleado;

            FechaPrestamo = PrestamoSeleccionado.FechaPrestamo;
            FechaDevolucion = PrestamoSeleccionado.FechaDevolucion;

            Devuelto = PrestamoSeleccionado.Devuelto;
            Mensaje = "Editando préstamo seleccionado.";
        }

        public void Limpiar()
        {
            PrestamoSeleccionado = null;

            LibroId = null;
            ClienteId = EsEmpleado ? (int?)null : clienteActualId;
            EmpleadoId = EsEmpleado ? (int?)null : (Empleados.Count > 0 ? Empleados[0].IdEmpleado : (int?)null);

            FechaPrestamo = EsEmpleado ? (DateTime?)null : DateTime.Today;
            FechaDevolucion = null;

            Devuelto = false;
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
            if (!PuedeEditar)
            {
                Mensaje = "No tienes permisos para crear préstamos.";
                return;
            }

            if (!EsEmpleado)
            {
                ClienteId = clienteActualId;
                if (Empleados.Count > 0 && !EmpleadoId.HasValue)
                    EmpleadoId = Empleados[0].IdEmpleado;
                if (!FechaPrestamo.HasValue)
                    FechaPrestamo = DateTime.Today;
            }

            DateTime fp = FechaPrestamo ?? default(DateTime);
            if (!service.ValidarAlta(GetLibroObj(), GetClienteObj(), GetEmpleadoObj(), fp, out string error))
            {
                Mensaje = error;
                return;
            }

            int idLibro = LibroId.Value;

            if (!service.PuedePrestar(idLibro, out error))
            {
                Mensaje = error;
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

            Mensaje = "Préstamo añadido correctamente.";
            Limpiar();
            CargarPrestamos();
            CargarCombos();
        }

        public void Update()
        {
            if (!PuedeEditar)
            {
                Mensaje = "No tienes permisos para modificar préstamos.";
                return;
            }

            if (PrestamoSeleccionado == null)
            {
                Mensaje = "Selecciona un préstamo.";
                return;
            }

            if (!EsEmpleado && clienteActualId.HasValue && PrestamoSeleccionado.IdCliente != clienteActualId.Value)
            {
                Mensaje = "No tienes permiso para modificar este préstamo.";
                return;
            }

            if (!EsEmpleado)
            {
                ClienteId = clienteActualId;
                if (Empleados.Count > 0 && !EmpleadoId.HasValue)
                    EmpleadoId = Empleados[0].IdEmpleado;
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
                    Mensaje = err;
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
                    Mensaje = err;
                    return;
                }
                service.AplicarPrestamoStock(newLibro);
            }

            repoPrestamos.Update(PrestamoSeleccionado);

            Mensaje = "Préstamo modificado correctamente.";
            Limpiar();
            CargarPrestamos();
            CargarCombos();
        }

        public void Delete()
        {
            if (!PuedeEditar)
            {
                Mensaje = "No tienes permisos para eliminar préstamos.";
                return;
            }

            if (PrestamoSeleccionado == null)
            {
                Mensaje = "Selecciona un préstamo.";
                return;
            }

            if (!EsEmpleado && clienteActualId.HasValue && PrestamoSeleccionado.IdCliente != clienteActualId.Value)
            {
                Mensaje = "No tienes permiso para eliminar este préstamo.";
                return;
            }

            if (!PrestamoSeleccionado.Devuelto)
                service.AplicarDevolucionStock(PrestamoSeleccionado.IdLibro);

            repoPrestamos.Delete(PrestamoSeleccionado.IdPrestamo);

            Mensaje = "Préstamo eliminado correctamente.";
            Limpiar();
            CargarPrestamos();
            CargarCombos();
        }
    }
}
