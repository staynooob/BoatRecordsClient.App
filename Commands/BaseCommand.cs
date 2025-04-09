using System.Windows.Input;

namespace BoatRecords.Commands;

public abstract class BaseCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }

    public abstract void Execute(object? parameter);

    protected void OnCanExecuteChanged(object? parameter)
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}
