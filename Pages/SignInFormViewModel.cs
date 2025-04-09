using BoatRecords.Commands;
using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BoatRecords.Pages;

partial class SignInFormViewModel : BaseViewModel
{
    private string _username = "";
    private string _password = "";
    private SignInFormViewModelCommand SignInFormViewModelCommand { get; }

    public string Username
    {
        get { return _username; }
        set
        {
            _username = value;
            OnPropertyChange(nameof(Username));
        }
    }

    public string Password
    {
        get { return _password; }
        set
        {
            _password = value;
            OnPropertyChange(nameof(Password));
        }
    }

    public SignInFormViewModel(SignedUserStorage signedUserStorage)
    {
        SignInFormViewModelCommand = new SignInFormViewModelCommand(this, signedUserStorage);
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private void SignIn()
    {
        SignInFormViewModelCommand.Execute(null);
    }
}
