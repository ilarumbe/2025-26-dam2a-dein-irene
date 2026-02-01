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

        private string mensaje;
        public string Mensaje
        {
            get => mensaje;
            set { mensaje = value; OnPropertyChanged(); }
        }

        public bool PuedeEditar => service.PuedeEditar();

        public CategoriasViewModel()
        {
            Cargar();
            Mensaje = "Selecciona una categoría o crea una nueva.";
        }

        public void Cargar()
        {
            Categorias.Clear();
            foreach (var c in repo.GetAll())
                Categorias.Add(c);
        }

        private void CargarDesdeSeleccion()
        {
            if (CategoriaSeleccionada == null) return;

            Nombre = CategoriaSeleccionada.Nombre;
            Mensaje = "Editando categoría seleccionada.";
        }

        public void Limpiar()
        {
            CategoriaSeleccionada = null;
            Nombre = "";
        }

        public void Add()
        {
            if (!service.ValidarAlta(Nombre, out string error))
            {
                Mensaje = error;
                return;
            }

            repo.Insert(new Categoria
            {
                Nombre = Nombre.Trim()
            });

            Mensaje = "Categoría añadida correctamente.";
            Limpiar();
            Cargar();
        }

        public void Update()
        {
            if (CategoriaSeleccionada == null)
            {
                Mensaje = "Selecciona una categoría.";
                return;
            }

            int? id = CategoriaSeleccionada.IdCategoria;
            if (!service.ValidarModificacion(id, Nombre, out string error))
            {
                Mensaje = error;
                return;
            }

            CategoriaSeleccionada.Nombre = Nombre.Trim();
            repo.Update(CategoriaSeleccionada);

            Mensaje = "Categoría modificada correctamente.";
            Limpiar();
            Cargar();
        }

        public void Delete()
        {
            if (CategoriaSeleccionada == null)
            {
                Mensaje = "Selecciona una categoría.";
                return;
            }

            int? id = CategoriaSeleccionada.IdCategoria;
            if (!service.ValidarBorrado(id, out string error))
            {
                Mensaje = error;
                return;
            }

            repo.Delete(CategoriaSeleccionada.IdCategoria);

            Mensaje = "Categoría eliminada correctamente.";
            Limpiar();
            Cargar();
        }
    }
}
