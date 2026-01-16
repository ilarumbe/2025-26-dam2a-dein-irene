using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace P2526_Irene_Biblioteca.UserControls
{
    public partial class PrestamosView : UserControl
    {
        private readonly PrestamosRepository repoPrestamos = new PrestamosRepository();
        private readonly LibrosRepository repoLibros = new LibrosRepository();
        private readonly ClientesRepository repoClientes = new ClientesRepository();
        private readonly EmpleadosRepository repoEmpleados = new EmpleadosRepository();
        private readonly PrestamosService service = new PrestamosService();

        private Prestamo prestamoSeleccionado;

        public PrestamosView()
        {
            InitializeComponent();
            CargarCombos();
            Cargar();
            AplicarPermisos();
        }

        private void AplicarPermisos()
        {
            bool puede = service.PuedeEditar();

            LibrosCombo.IsEnabled = puede;
            ClientesCombo.IsEnabled = puede;
            EmpleadosCombo.IsEnabled = puede;
            FechaPrestamoPicker.IsEnabled = puede;
            FechaDevolucionPicker.IsEnabled = puede;
            DevueltoCheck.IsEnabled = puede;
        }

        private void CargarCombos()
        {
            LibrosCombo.ItemsSource = repoLibros.GetAll();
            ClientesCombo.ItemsSource = repoClientes.GetAll();
            EmpleadosCombo.ItemsSource = repoEmpleados.GetAll();
        }

        private void Cargar()
        {
            List<Prestamo> lista = repoPrestamos.GetAll();
            PrestamosGrid.ItemsSource = lista;
        }

        private void PrestamosGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            prestamoSeleccionado = PrestamosGrid.SelectedItem as Prestamo;
            if (prestamoSeleccionado == null) return;

            LibrosCombo.SelectedValue = prestamoSeleccionado.IdLibro;
            ClientesCombo.SelectedValue = prestamoSeleccionado.IdCliente;
            EmpleadosCombo.SelectedValue = prestamoSeleccionado.IdEmpleado;

            FechaPrestamoPicker.SelectedDate = prestamoSeleccionado.FechaPrestamo;
            FechaDevolucionPicker.SelectedDate = prestamoSeleccionado.FechaDevolucion;

            DevueltoCheck.IsChecked = prestamoSeleccionado.Devuelto;
            ErrorText.Text = "";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            DateTime fechaPrestamo = FechaPrestamoPicker.SelectedDate ?? default;
            if (!service.ValidarAlta(LibrosCombo.SelectedItem, ClientesCombo.SelectedItem, EmpleadosCombo.SelectedItem, fechaPrestamo, out string error))
            {
                ErrorText.Text = error;
                return;
            }

            int idLibro = (int)LibrosCombo.SelectedValue;

            if (!service.PuedePrestar(idLibro, out error))
            {
                ErrorText.Text = error;
                return;
            }

            repoPrestamos.Insert(new Prestamo
            {
                IdLibro = idLibro,
                IdCliente = (int)ClientesCombo.SelectedValue,
                IdEmpleado = (int)EmpleadosCombo.SelectedValue,
                FechaPrestamo = fechaPrestamo,
                FechaDevolucion = FechaDevolucionPicker.SelectedDate,
                Devuelto = DevueltoCheck.IsChecked == true
            });

            if (DevueltoCheck.IsChecked != true)
                service.AplicarPrestamoStock(idLibro);

            BtnClear_Click(sender, e);
            Cargar();
            CargarCombos();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";
            if (prestamoSeleccionado == null)
            {
                ErrorText.Text = "Selecciona un préstamo.";
                return;
            }

            int oldLibro = prestamoSeleccionado.IdLibro;
            bool oldDevuelto = prestamoSeleccionado.Devuelto;

            prestamoSeleccionado.IdLibro = (int)LibrosCombo.SelectedValue;
            prestamoSeleccionado.IdCliente = (int)ClientesCombo.SelectedValue;
            prestamoSeleccionado.IdEmpleado = (int)EmpleadosCombo.SelectedValue;
            prestamoSeleccionado.FechaPrestamo = FechaPrestamoPicker.SelectedDate ?? prestamoSeleccionado.FechaPrestamo;
            prestamoSeleccionado.FechaDevolucion = FechaDevolucionPicker.SelectedDate;
            prestamoSeleccionado.Devuelto = DevueltoCheck.IsChecked == true;

            if (!oldDevuelto && prestamoSeleccionado.Devuelto)
                service.AplicarDevolucionStock(oldLibro);

            if (oldDevuelto && !prestamoSeleccionado.Devuelto)
            {
                if (!service.PuedePrestar(prestamoSeleccionado.IdLibro, out string err))
                {
                    ErrorText.Text = err;
                    return;
                }
                service.AplicarPrestamoStock(prestamoSeleccionado.IdLibro);
            }

            if (!prestamoSeleccionado.Devuelto && oldLibro != prestamoSeleccionado.IdLibro)
            {
                service.AplicarDevolucionStock(oldLibro);

                if (!service.PuedePrestar(prestamoSeleccionado.IdLibro, out string err))
                {
                    service.AplicarPrestamoStock(oldLibro);
                    ErrorText.Text = err;
                    return;
                }
                service.AplicarPrestamoStock(prestamoSeleccionado.IdLibro);
            }

            repoPrestamos.Update(prestamoSeleccionado);

            BtnClear_Click(sender, e);
            Cargar();
            CargarCombos();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";
            if (prestamoSeleccionado == null)
            {
                ErrorText.Text = "Selecciona un préstamo.";
                return;
            }

            if (MessageBox.Show("¿Seguro que quieres eliminar este préstamo?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            if (!prestamoSeleccionado.Devuelto)
                service.AplicarDevolucionStock(prestamoSeleccionado.IdLibro);

            repoPrestamos.Delete(prestamoSeleccionado.IdPrestamo);

            BtnClear_Click(sender, e);
            Cargar();
            CargarCombos();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            prestamoSeleccionado = null;
            PrestamosGrid.SelectedItem = null;

            LibrosCombo.SelectedIndex = -1;
            ClientesCombo.SelectedIndex = -1;
            EmpleadosCombo.SelectedIndex = -1;

            FechaPrestamoPicker.SelectedDate = null;
            FechaDevolucionPicker.SelectedDate = null;

            DevueltoCheck.IsChecked = false;
            ErrorText.Text = "";
        }
    }
}
