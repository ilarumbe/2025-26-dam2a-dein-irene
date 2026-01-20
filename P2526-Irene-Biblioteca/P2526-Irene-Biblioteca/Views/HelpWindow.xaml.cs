using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Services;
using System.Windows;

namespace P2526_Irene_Biblioteca.Views
{
    public partial class HelpWindow : Window
    {
        public HelpWindow(string pantallaActual)
        {
            InitializeComponent();

            TitleText.Text = "Ayuda";
            SubTitleText.Text = "Pantalla actual: " + (string.IsNullOrWhiteSpace(pantallaActual) ? "Biblioteca" : pantallaActual);

            RoleText.Text = SesionActual.EsEmpleado
                ? "Empleado (acceso completo)."
                : "Cliente (solo tus préstamos).";

            ContextHelpText.Text = GetHelpForScreen(pantallaActual);
        }

        private string GetHelpForScreen(string pantalla)
        {
            if (string.IsNullOrWhiteSpace(pantalla))
                pantalla = "Biblioteca";

            switch (pantalla)
            {
                case "Autores":
                    return "Selecciona un autor para editarlo. Modifica Nombre/Nacionalidad y pulsa Modificar. Para crear uno nuevo, rellena el formulario y pulsa Añadir. Usa Eliminar para borrarlo.";
                case "Libros":
                    return "Usa el buscador para filtrar. Selecciona un libro con el botón Seleccionar para editarlo. Elige Autor y Categoría desde los desplegables para evitar errores. Stock y Año deben ser numéricos.";
                case "Categorías":
                    return "Haz clic en una categoría para editarla. Cambia el nombre y pulsa Modificar. Para crear una categoría nueva, escribe un nombre y pulsa Añadir.";
                case "Clientes":
                    return "Selecciona un cliente para editarlo. La contraseña solo se cambia si escribes una nueva. Si la dejas vacía, no se modifica.";
                case "Empleados":
                    return "Selecciona un empleado para editarlo. La contraseña solo se cambia si escribes una nueva. Si la dejas vacía, no se modifica.";
                case "Préstamos":
                    if (SesionActual.EsEmpleado)
                        return "Como empleado, puedes gestionar todos los préstamos. Selecciona uno en el histórico para editar o eliminar. Para crear uno nuevo, elige Libro/Cliente/Empleado y fechas y pulsa Añadir.";
                    return "Como cliente, solo ves y gestionas tus préstamos. Puedes crear un préstamo eligiendo un libro y una fecha. También puedes modificar o eliminar únicamente los tuyos.";
                default:
                    return "Usa el menú lateral para navegar. En cada pantalla, selecciona un elemento para editar o rellena el formulario para crear uno nuevo.";
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
