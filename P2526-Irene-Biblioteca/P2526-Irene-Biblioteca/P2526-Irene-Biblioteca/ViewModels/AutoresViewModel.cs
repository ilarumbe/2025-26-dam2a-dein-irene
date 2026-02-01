using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.ObjectModel;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class AutoresViewModel : BaseViewModel
    {
        private readonly AutoresRepository repo = new AutoresRepository();
        private readonly AutoresService service = new AutoresService();

        public ObservableCollection<Autor> Autores { get; } = new ObservableCollection<Autor>();

        private Autor autorSeleccionado;
        public Autor AutorSeleccionado
        {
            get => autorSeleccionado;
            set
            {
                autorSeleccionado = value;
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

        private string nacionalidad;
        public string Nacionalidad
        {
            get => nacionalidad;
            set { nacionalidad = value; OnPropertyChanged(); }
        }

        private string mensaje;
        public string Mensaje
        {
            get => mensaje;
            set { mensaje = value; OnPropertyChanged(); }
        }

        public bool PuedeEditar => service.PuedeEditar();

        public AutoresViewModel()
        {
            Cargar();
            Mensaje = "Selecciona un autor o crea uno nuevo.";
        }

        public void Cargar()
        {
            Autores.Clear();
            foreach (var a in repo.GetAll())
                Autores.Add(a);

        }

        private void CargarDesdeSeleccion()
        {
            if (AutorSeleccionado == null) return;

            Nombre = AutorSeleccionado.Nombre;
            Nacionalidad = AutorSeleccionado.Nacionalidad;

            Mensaje = "Editando autor seleccionado.";
        }

        public void Limpiar()
        {
            AutorSeleccionado = null;
            Nombre = "";
            Nacionalidad = "";
        }

        public void Add()
        {
            if (!service.ValidarAlta(Nombre, out string error))
            {
                Mensaje = error;
                return;
            }

            repo.Insert(new Autor
            {
                Nombre = Nombre.Trim(),
                Nacionalidad = string.IsNullOrWhiteSpace(Nacionalidad) ? null : Nacionalidad.Trim()
            });

            Mensaje = "Autor añadido correctamente.";
            Limpiar();
            Cargar();
        }

        public void Update()
        {
            if (AutorSeleccionado == null)
            {
                Mensaje = "Selecciona un autor.";
                return;
            }

            int? id = AutorSeleccionado.IdAutor;
            if (!service.ValidarModificacion(id, Nombre, out string error))
            {
                Mensaje = error;
                return;
            }

            AutorSeleccionado.Nombre = Nombre.Trim();
            AutorSeleccionado.Nacionalidad = string.IsNullOrWhiteSpace(Nacionalidad) ? null : Nacionalidad.Trim();

            repo.Update(AutorSeleccionado);

            Mensaje = "Autor modificado correctamente.";
            Limpiar();
            Cargar();
        }

        public bool CanDelete(out string error)
        {
            int? id = AutorSeleccionado?.IdAutor;
            return service.ValidarBorrado(id, out error);
        }

        public void Delete()
        {
            if (AutorSeleccionado == null)
            {
                Mensaje = "Selecciona un autor.";
                return;
            }

            if (!CanDelete(out string error))
            {
                Mensaje = error;
                return;
            }

            repo.Delete(AutorSeleccionado.IdAutor);

            Mensaje = "Autor eliminado correctamente.";
            Limpiar();
            Cargar();
        }
    }
}
