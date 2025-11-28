using P2526_Irene_Biblioteca.Helpers;
using P2526_Irene_Biblioteca.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2526_Irene_Biblioteca.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly IWindowService windowService;

        public ICommand CloseCommand { get; }

        public MainWindowViewModel(IWindowService service)
        {
            windowService = service;
            CloseCommand = new RelayCommand(o => windowService.Close());
        }
    }
}
