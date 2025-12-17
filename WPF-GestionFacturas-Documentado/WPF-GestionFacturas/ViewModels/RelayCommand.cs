using System;
using System.Windows.Input;

namespace WPF_GestionFacturas.ViewModels
{
    /// <summary>
    /// Implementación estándar de la interfaz <see cref="ICommand"/> diseñada para el patrón MVVM.
    /// <para>
    /// Su propósito es "retransmitir" (relay) la ejecución de comandos a delegados (Action y Predicate) 
    /// definidos en el ViewModel. Esto evita tener que crear una clase distinta por cada comando de la aplicación.
    /// </para>
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Acción que se ejecutará cuando se invoque el comando.
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// Predicado que determina si el comando puede ejecutarse o no.
        /// </summary>
        private readonly Predicate<object> _canExecute;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">
        /// La lógica de ejecución (Action) que se llamará al pulsar el botón o invocar el comando.
        /// </param>
        /// <param name="canExecute">
        /// La lógica de validación (Predicate) que determina si el comando está habilitado.
        /// Si se pasa <c>null</c>, el comando siempre estará habilitado (devuelve <c>true</c>).
        /// </param>
        /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="execute"/> es nulo.</exception>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determina si el comando puede ejecutarse en el estado actual.
        /// </summary>
        /// <param name="parameter">Datos opcionales pasados por el comando. Si no se usan, puede ser <c>null</c>.</param>
        /// <returns>
        /// <c>true</c> si el comando puede ejecutarse; de lo contrario, <c>false</c>.
        /// </returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        /// <summary>
        /// Ejecuta la lógica del comando.
        /// </summary>
        /// <param name="parameter">Datos opcionales pasados por el comando.</param>
        public void Execute(object parameter) => _execute(parameter);

        /// <summary>
        /// Se produce cuando ocurren cambios que influyen en si el comando debería ejecutarse o no.
        /// </summary>
        /// <remarks>
        /// Este evento se engancha al <see cref="CommandManager.RequerySuggested"/> de WPF.
        /// Esto significa que la interfaz gráfica (UI) preguntará automáticamente si el botón debe estar habilitado
        /// cada vez que el usuario interactúe con la ventana (ej. cambiar foco, escribir texto, etc.).
        /// </remarks>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}