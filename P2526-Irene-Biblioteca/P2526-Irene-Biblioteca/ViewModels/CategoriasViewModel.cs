using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.ObjectModel;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class CategoriasViewModel : BaseViewModel
    {
        private readonly CategoriasRepository repo = new CategoriasRepository();
        private readonly CategoriasService service = new CategoriasService();

        public ObservableCollection<Categoria> Categorias { get; } = new ObservableCollection<Categoria>();

        private Categoria categoriaSeleccionada;
        public Categoria CategoriaSeleccionada
        {
            get => categoriaSeleccionada;
            set
            {
                categoriaSeleccionada = value;
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

        private string errorText;
        public string ErrorText
        {
            get => errorText;
            set { errorText = value; OnPropertyChanged(); }
        }

        public bool PuedeEditar => service.PuedeEditar();

        public CategoriasViewModel()
        {
            Cargar();
        }

        public void Cargar()
        {
            Categorias.Clear();
            foreach (var c in repo.GetAll())
                Categorias.Add(c);

            ErrorText = "";
        }

        private void CargarDesdeSeleccion()
        {
            if (CategoriaSeleccionada == null) return;

            Nombre = CategoriaSeleccionada.Nombre;
            ErrorText = "";
        }

        public void Limpiar()
        {
            CategoriaSeleccionada = null;
            Nombre = "";
            ErrorText = "";
        }

        public void Add()
        {
            ErrorText = "";

            if (!service.ValidarAlta(Nombre, out string error))
            {
                ErrorText = error;
                return;
            }

            repo.Insert(new Categoria
            {
                Nombre = Nombre.Trim()
            });

            Limpiar();
            Cargar();
        }

        public void Update()
        {
            ErrorText = "";

            int? id = CategoriaSeleccionada?.IdCategoria;
            if (!service.ValidarModificacion(id, Nombre, out string error))
            {
                ErrorText = error;
                return;
            }

            CategoriaSeleccionada.Nombre = Nombre.Trim();
            repo.Update(CategoriaSeleccionada);

            Limpiar();
            Cargar();
        }

        public void Delete()
        {
            ErrorText = "";

            int? id = CategoriaSeleccionada?.IdCategoria;
            if (!service.ValidarBorrado(id, out string error))
            {
                ErrorText = error;
                return;
            }

            repo.Delete(CategoriaSeleccionada.IdCategoria);

            Limpiar();
            Cargar();
        }
    }
}
