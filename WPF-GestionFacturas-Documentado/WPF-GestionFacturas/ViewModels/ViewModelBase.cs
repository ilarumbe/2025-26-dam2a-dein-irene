using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_GestionFacturas.ViewModels
{
    /// <summary>
    /// Clase base abstracta que implementa la infraestructura necesaria para el patrón MVVM.
    /// <para>
    /// Implementa la interfaz <see cref="INotifyPropertyChanged"/>, que es el mecanismo estándar de WPF
    /// para notificar a la Vista (XAML) que los datos en el ViewModel han cambiado y deben redibujarse.
    /// Todos los ViewModels de la aplicación deben heredar de esta clase.
    /// </para>
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Evento que se produce cuando cambia el valor de una propiedad.
        /// </summary>
        /// <remarks>
        /// El sistema de DataBinding de WPF se suscribe automáticamente a este evento.
        /// </remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Método auxiliar para lanzar el evento <see cref="PropertyChanged"/> de forma sencilla.
        /// </summary>
        /// <param name="propertyName">
        /// El nombre de la propiedad que ha cambiado. 
        /// <para>
        /// Gracias al atributo <see cref="CallerMemberName"/>, este parámetro es opcional.
        /// El compilador inyectará automáticamente el nombre de la propiedad desde la que se llama a este método,
        /// evitando errores tipográficos ("Magic Strings").
        /// </para>
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}