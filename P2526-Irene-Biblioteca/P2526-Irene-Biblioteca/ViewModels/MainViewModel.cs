using P2526_Irene_Biblioteca.UserControls;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private object currentView;
        public object CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NombreVistaActual));
            }
        }

        public string NombreVistaActual
        {
            get
            {
                if (CurrentView == null) return "Biblioteca";

                var typeName = CurrentView.GetType().Name;

                if (typeName == nameof(AutoresView)) return "Autores";
                if (typeName == nameof(LibrosView)) return "Libros";
                if (typeName == nameof(CategoriasView)) return "Categorías";
                if (typeName == nameof(ClientesView)) return "Clientes";
                if (typeName == nameof(EmpleadosView)) return "Empleados";
                if (typeName == nameof(PrestamosView)) return "Préstamos";

                return "Biblioteca";
            }
        }

        public MainWindowViewModel()
        {
            ShowAutores();
        }

        public void ShowAutores() => CurrentView = new AutoresView();
        public void ShowLibros() => CurrentView = new LibrosView();
        public void ShowCategorias() => CurrentView = new CategoriasView();
        public void ShowClientes() => CurrentView = new ClientesView();
        public void ShowEmpleados() => CurrentView = new EmpleadosView();
        public void ShowPrestamos() => CurrentView = new PrestamosView();
    }
}
