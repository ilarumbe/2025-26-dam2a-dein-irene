using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
            }
        }

        public ICommand ShowAutoresCommand { get; }
        public ICommand ShowLibrosCommand { get; }
        public ICommand ShowCategoriasCommand { get; }
        public ICommand ShowClientesCommand { get; }
        public ICommand ShowEmpleadosCommand { get; }
        public ICommand ShowPrestamosCommand { get; }

        public ICommand CloseCommand { get; }

        public MainWindowViewModel()
        {
            ShowAutoresCommand = new RelayCommand(_ => CurrentView = new AutoresView());
            ShowLibrosCommand = new RelayCommand(_ => CurrentView = new LibrosView());
            ShowCategoriasCommand = new RelayCommand(_ => CurrentView = new CategoriasView());
            ShowClientesCommand = new RelayCommand(_ => CurrentView = new ClientesView());
            ShowEmpleadosCommand = new RelayCommand(_ => CurrentView = new EmpleadosView());
            ShowPrestamosCommand = new RelayCommand(_ => CurrentView = new PrestamosView());

            CloseCommand = new RelayCommand(_ => System.Windows.Application.Current.Shutdown());

            CurrentView = new AutoresView();
        }
    }
}