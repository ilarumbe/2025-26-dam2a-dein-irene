using P2526_Irene_Biblioteca.Models;
using P2526_Irene_Biblioteca.Repositories;
using P2526_Irene_Biblioteca.Services;
using System.Collections.ObjectModel;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class LibrosViewModel : BaseViewModel
    {
        private readonly LibrosRepository repoLibros = new LibrosRepository();
        private readonly AutoresRepository repoAutores = new AutoresRepository();
        private readonly CategoriasRepository repoCategorias = new CategoriasRepository();
        private readonly LibrosService service = new LibrosService();

        public ObservableCollection<Libro> Libros { get; } = new ObservableCollection<Libro>();
        public ObservableCollection<Autor> Autores { get; } = new ObservableCollection<Autor>();
        public ObservableCollection<Categoria> Categorias { get; } = new ObservableCollection<Categoria>();

        private Libro libroSeleccionado;
        public Libro LibroSeleccionado
        {
            get => libroSeleccionado;
            set
            {
                libroSeleccionado = value;
                OnPropertyChanged();
                CargarDesdeSeleccion();
            }
        }

        private string textoBusqueda;
        public string TextoBusqueda
        {
            get => textoBusqueda;
            set
            {
                textoBusqueda = value;
                OnPropertyChanged();
                Filtrar();
            }
        }

        private string titulo;
        public string Titulo
        {
            get => titulo;
            set { titulo = value; OnPropertyChanged(); }
        }

        private string anio;
        public string Anio
        {
            get => anio;
            set { anio = value; OnPropertyChanged(); }
        }

        private int? autorId;
        public int? AutorId
        {
            get => autorId;
            set { autorId = value; OnPropertyChanged(); }
        }

        private int? categoriaId;
        public int? CategoriaId
        {
            get => categoriaId;
            set { categoriaId = value; OnPropertyChanged(); }
        }

        private string stock;
        public string Stock
        {
            get => stock;
            set { stock = value; OnPropertyChanged(); }
        }

        private string errorText;
        public string ErrorText
        {
            get => errorText;
            set { errorText = value; OnPropertyChanged(); }
        }

        public bool PuedeEditar => service.PuedeEditar();

        private readonly ObservableCollection<Libro> librosAll = new ObservableCollection<Libro>();

        public LibrosViewModel()
        {
            CargarCombos();
            CargarLibros();
        }

        public void CargarCombos()
        {
            Autores.Clear();
            foreach (var a in repoAutores.GetAll())
                Autores.Add(a);

            Categorias.Clear();
            foreach (var c in repoCategorias.GetAll())
                Categorias.Add(c);
        }

        public void CargarLibros()
        {
            Libros.Clear();
            librosAll.Clear();

            foreach (var l in repoLibros.GetAll())
            {
                Libros.Add(l);
                librosAll.Add(l);
            }

            ErrorText = "";
        }

        private void Filtrar()
        {
            string q = (TextoBusqueda ?? "").Trim().ToLower();

            Libros.Clear();

            if (q.Length == 0)
            {
                foreach (var l in librosAll) Libros.Add(l);
                return;
            }

            foreach (var l in librosAll)
            {
                string titulo = (l.Titulo ?? "").ToLower();
                string autor = (l.AutorNombre ?? "").ToLower();
                string cat = (l.CategoriaNombre ?? "").ToLower();

                if (titulo.Contains(q) || autor.Contains(q) || cat.Contains(q))
                    Libros.Add(l);
            }
        }

        private void CargarDesdeSeleccion()
        {
            if (LibroSeleccionado == null) return;

            Titulo = LibroSeleccionado.Titulo;
            Anio = LibroSeleccionado.Año.ToString();
            Stock = LibroSeleccionado.Stock.ToString();

            AutorId = LibroSeleccionado.IdAutor;
            CategoriaId = LibroSeleccionado.IdCategoria;

            ErrorText = "";
        }

        public void Limpiar()
        {
            LibroSeleccionado = null;
            Titulo = "";
            Anio = "";
            Stock = "";
            AutorId = null;
            CategoriaId = null;
            ErrorText = "";
        }

        public void Add()
        {
            ErrorText = "";

            var autorObj = BuscarAutorSeleccionado();
            var catObj = BuscarCategoriaSeleccionada();

            if (!service.ValidarAlta(Titulo, Anio, autorObj, catObj, Stock, out string error))
            {
                ErrorText = error;
                return;
            }

            repoLibros.Insert(new Libro
            {
                Titulo = Titulo.Trim(),
                Año = int.Parse(Anio),
                IdAutor = AutorId.Value,
                IdCategoria = CategoriaId.Value,
                Stock = int.Parse(Stock)
            });

            Limpiar();
            CargarLibros();
        }

        public void Update()
        {
            ErrorText = "";

            int? id = LibroSeleccionado?.IdLibro;

            var autorObj = BuscarAutorSeleccionado();
            var catObj = BuscarCategoriaSeleccionada();

            if (!service.ValidarModificacion(id, Titulo, Anio, autorObj, catObj, Stock, out string error))
            {
                ErrorText = error;
                return;
            }

            LibroSeleccionado.Titulo = Titulo.Trim();
            LibroSeleccionado.Año = int.Parse(Anio);
            LibroSeleccionado.IdAutor = AutorId.Value;
            LibroSeleccionado.IdCategoria = CategoriaId.Value;
            LibroSeleccionado.Stock = int.Parse(Stock);

            repoLibros.Update(LibroSeleccionado);

            Limpiar();
            CargarLibros();
        }

        public void Delete()
        {
            ErrorText = "";

            int? id = LibroSeleccionado?.IdLibro;
            if (!service.ValidarBorrado(id, out string error))
            {
                ErrorText = error;
                return;
            }

            repoLibros.Delete(LibroSeleccionado.IdLibro);

            Limpiar();
            CargarLibros();
        }

        private Autor BuscarAutorSeleccionado()
        {
            if (!AutorId.HasValue) return null;
            foreach (var a in Autores)
                if (a.IdAutor == AutorId.Value) return a;
            return null;
        }

        private Categoria BuscarCategoriaSeleccionada()
        {
            if (!CategoriaId.HasValue) return null;
            foreach (var c in Categorias)
                if (c.IdCategoria == CategoriaId.Value) return c;
            return null;
        }
    }
}
