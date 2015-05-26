using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Nito.AsyncEx;

namespace DemoMvvm
{
    public class AsyncCancellableCommand<TParameter> : AsyncCommandBase, IAsyncCancellableCommand
    {
        #region Fields

        private readonly Func<TParameter, CancellationToken, Task> execute;
        private readonly Func<TParameter, bool> canExecute;
        private readonly bool blocking;
        private readonly CancelAsyncCommand cancelCommand;

        #endregion

        #region Constructors and Finalizers

        public AsyncCancellableCommand(Func<TParameter, CancellationToken, Task> execute, Func<TParameter, bool> canExecute, bool blocking = true)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
            this.blocking = blocking;
            this.cancelCommand = new CancelAsyncCommand();
        }

        public AsyncCancellableCommand(Func<TParameter, CancellationToken, Task> execute, bool blocking = true)
            : this(execute, null, blocking)
        {
        }

        #endregion

        #region Properties

        public ICommand CancelCommand
        {
            get { return cancelCommand; }
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
                cancelCommand.NotifyCommandStarting();
                Execution = NotifyTaskCompletion.Create(execute((TParameter)parameter, cancelCommand.Token));
                RaiseCanExecuteChanged();
                await Execution.Task;
                RaiseCanExecuteChanged();
                cancelCommand.NotifyCommandFinished();
            }
        }

        #endregion

        #region Nested Types

        private sealed class CancelAsyncCommand : ICommand
        {
            #region Fields

            private CancellationTokenSource cts = new CancellationTokenSource();
            private bool commandExecuting;

            #endregion

            #region Properties

            public CancellationToken Token
            {
                get { return cts.Token; }
            }

            #endregion

            #region Methods

            public void NotifyCommandStarting()
            {
                commandExecuting = true;
                if (cts.IsCancellationRequested)
                {
                    cts = new CancellationTokenSource();
                }
                RaiseCanExecuteChanged();
            }

            public void NotifyCommandFinished()
            {
                commandExecuting = false;
                RaiseCanExecuteChanged();
            }

            bool ICommand.CanExecute(object parameter)
            {
                return commandExecuting && !cts.IsCancellationRequested;
            }

            void ICommand.Execute(object parameter)
            {
                cts.Cancel();
                RaiseCanExecuteChanged();
            }

            private void RaiseCanExecuteChanged()
            {
                var handler = CanExecuteChanged;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }

            #endregion

            #region Events

            public event EventHandler CanExecuteChanged;

            #endregion
        }

        #endregion
    }

    public class AsyncCancellableCommand : AsyncCancellableCommand<object>
    {
        public AsyncCancellableCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute, bool blocking = true) :
            base((x, t) => execute(t), x => canExecute(), blocking)
        {
        }

        public AsyncCancellableCommand(Func<CancellationToken, Task> execute, bool blocking = true)
            : base((x, t) => execute(t), blocking)
        {
        }
    }
}
