using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace DemoMvvm
{
    public class AsyncCommand<TParameter> : AsyncCommandBase, IAsyncCommand
    {
        #region Fields

        private readonly Func<TParameter, Task> execute;
        private readonly Func<TParameter, bool> canExecute;
        private readonly bool blocking;

        #endregion

        #region Constructors and Finalizers

        public AsyncCommand(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute, bool blocking = true)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
            this.blocking = blocking;
        }

        public AsyncCommand(Func<TParameter, Task> execute, bool blocking = true)
            : this(execute, null, blocking)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this object can
        /// be set to null.
        /// </param>
        public override bool CanExecute(object parameter)
        {
            var execution = Execution;
            return (!blocking || execution == null || execution.IsCompleted) && (canExecute == null || canExecute((TParameter)parameter));
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                Execution = NotifyTaskCompletion.Create(execute((TParameter)parameter));
                RaiseCanExecuteChanged();
                await Execution.Task;
                RaiseCanExecuteChanged();
            }
        }

        #endregion
    }

    public class AsyncCommand : AsyncCommand<object>
    {
        public AsyncCommand(Func<Task> execute, Func<bool> canExecute, bool blocking = true) :
            base(x => execute(), x => canExecute(), blocking)
        {
        }

        public AsyncCommand(Func<Task> execute, bool blocking = true)
            : base(x => execute(), blocking)
        {
        }
    }
}
