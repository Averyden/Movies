using System.Windows.Input;

namespace TheMovies
{
    public class CommandHandler : ICommand
    {
        private Action _execute;
        public CommandHandler(Action command)
        {
            _execute = command;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute(); 
        }
    }
}
