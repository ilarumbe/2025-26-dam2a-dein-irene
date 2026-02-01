using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2526_Irene_Biblioteca.Services
{
    public class WindowService : IWindowService
    {
        private readonly Window window;

        public WindowService(Window w)
        {
            window = w;
        }

        public void Close()
        {
            window.Close();
        }
    }
}
