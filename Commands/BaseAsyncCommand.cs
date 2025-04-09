using Nito.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoatRecords.Commands;

public abstract class BaseAsyncCommand : BaseCommand
{
    private bool _isRunning;
    public bool IsRunning
    {
        get { return _isRunning; }
        set {
            _isRunning = value;
            OnCanExecuteChanged(null);
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return !IsRunning && base.CanExecute(parameter);
    }

    public override async void Execute(object param)
    {
        IsRunning = true;
        try
        {
            await ExecuteAsync(param);
        } finally
        {
            IsRunning = false;
        }
    }

    public abstract Task ExecuteAsync(object param);
}
