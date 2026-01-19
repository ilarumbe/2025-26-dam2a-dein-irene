using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Services;
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

        public bool EsEmpleado => SesionActual.EsEmpleado;
        public bool PuedeVerPrestamos => true;

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
            if (SesionActual.EsEmpleado)
                ShowAutores();
            else
                ShowPrestamos();
        }

        public void ShowAutores()
        {
            if (!SesionActual.EsEmpleado) return;
            CurrentView = new AutoresView();
        }

        public void ShowLibros()
        {
            if (!SesionActual.EsEmpleado) return;
            CurrentView = new LibrosView();
        }

        public void ShowCategorias()
        {
            if (!SesionActual.EsEmpleado) return;
            CurrentView = new CategoriasView();
        }

        public void ShowClientes()
        {
            if (!SesionActual.EsEmpleado) return;
            CurrentView = new ClientesView();
        }

        public void ShowEmpleados()
        {
            if (!SesionActual.EsEmpleado) return;
            CurrentView = new EmpleadosView();
        }

        public void ShowPrestamos()
        {
            CurrentView = new PrestamosView();
        }
    }
}
