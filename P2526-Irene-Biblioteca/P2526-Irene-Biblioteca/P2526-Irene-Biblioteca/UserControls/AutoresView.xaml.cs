using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace P2526_Irene_Biblioteca.UserControls
{
    /// <summary>
    /// Vista (UserControl) encargada de mostrar y gestionar autores.
    /// Contiene el código de interacción de la UI (eventos) y delega la lógica al ViewModel.
    /// </summary>
    public partial class AutoresView : UserControl
    {
        private AutoresViewModel VM => (AutoresViewModel)DataContext;

        /// <summary>
        /// Inicializa la vista de Autores y asigna su <see cref="AutoresViewModel"/> como DataContext.
        /// </summary>
        public AutoresView()
        {
            InitializeComponent();
            DataContext = new AutoresViewModel();
        }

        /// <summary>
        /// Selecciona el autor asociado al botón pulsado dentro del ItemsControl.
        /// </summary>
        /// <param name="sender">Elemento que dispara el evento (normalmente un Button dentro del DataTemplate).</param>
        /// <param name="e">Datos del evento de enrutamiento.</param>
        private void AutorSelect_Click(object sender, RoutedEventArgs e)
        {
            var autor = (sender as FrameworkElement)?.DataContext as Autor;
            if (autor == null) return;

            VM.AutorSeleccionado = autor;
        }
        
        /// <summary>
        /// Llama al ViewModel para añadir un autor con los datos del formulario.
        /// </summary>
        /// <param name="sender">Botón pulsado.</param>
        /// <param name="e">Datos del evento.</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            VM.Add();
        }

        /// <summary>
        /// Llama al ViewModel para modificar el autor seleccionado.
        /// </summary>
        /// <param name="sender">Botón pulsado.</param>
        /// <param name="e">Datos del evento.</param>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            VM.Update();
        }

        /// <summary>
        /// Solicita confirmación y, si el usuario acepta, elimina el autor seleccionado.
        /// </summary>
        /// <param name="sender">Botón pulsado.</param>
        /// <param name="e">Datos del evento.</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (VM.AutorSeleccionado == null)
            {
                VM.Mensaje = "Selecciona un autor.";
                return;
            }

            if (!ConfirmarBorrado("autor"))
                return;

            VM.Delete();
        }

        /// <summary>
        /// Limpia el formulario y deselecciona el autor actual.
        /// </summary>
        /// <param name="sender">Botón pulsado.</param>
        /// <param name="e">Datos del evento.</param>
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            VM.Limpiar();
        }

        /// <summary>
        /// Evento de cambio de texto (no usado actualmente).
        /// </summary>
        /// <param name="sender">TextBox que dispara el evento.</param>
        /// <param name="e">Datos del evento de texto.</param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Reservado por si se quiere validar en tiempo real.
        }

        /// <summary>
        /// Da foco al control al cargarse para permitir atajos de teclado.
        /// </summary>
        /// <param name="sender">Control que dispara el evento.</param>
        /// <param name="e">Datos del evento.</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Focus();
        }

        /// <summary>
        /// Gestiona atajos de teclado para acciones rápidas:
        /// Ctrl+S (Añadir), Ctrl+U (Modificar), Ctrl+D (Eliminar), Escape (Limpiar).
        /// </summary>
        /// <param name="sender">Control que recibe el evento.</param>
        /// <param name="e">Datos del evento de teclado.</param>
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!VM.PuedeEditar)
                return;

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                VM.Add();
                e.Handled = true;
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                VM.Update();
                e.Handled = true;
                return;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.D)
            {
                if (VM.AutorSeleccionado == null)
                {
                    VM.Mensaje = "Selecciona un autor.";
                    e.Handled = true;
                    return;
                }

                if (ConfirmarBorrado("autor"))
                    VM.Delete();

                e.Handled = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                VM.Limpiar();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Muestra un cuadro de confirmación antes de eliminar una entidad.
        /// </summary>
        /// <param name="entidad">Nombre de la entidad a eliminar (ej.: "autor").</param>
        /// <returns>
        /// true si el usuario confirma la operación; false si cancela.
        /// </returns>
        private bool ConfirmarBorrado(string entidad)
        {
            var r = MessageBox.Show(
                $"¿Seguro que quieres eliminar este {entidad}?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            return r == MessageBoxResult.Yes;
        }
    }
}
