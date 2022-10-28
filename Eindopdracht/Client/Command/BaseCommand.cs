using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers.Interfaces;

namespace Client.Commands;

public abstract class BaseCommand : IAsyncCommand
{
    
    public event EventHandler? CanExecuteChanged;
    
    public virtual bool CanExecute(object? parameter) => true;

    public abstract void Execute(object? parameter);

    protected void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }

    public abstract Task ExecuteAsync();
    
}