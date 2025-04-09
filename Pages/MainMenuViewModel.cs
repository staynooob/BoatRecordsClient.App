using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;

namespace BoatRecords.Pages;

partial class MainMenuViewModel : BaseViewModel
{
    private bool _isUserSignedIn = false;
    private SignedUserStorage _signedUserStorage;

    public bool IsUserSignedIn
    {
        get { return _isUserSignedIn; }
        set
        {
            _isUserSignedIn = value;
            OnPropertyChange(nameof(IsUserSignedIn));
        }
    }

    public MainMenuViewModel(SignedUserStorage signedUserStorage)
    {
        _signedUserStorage = signedUserStorage;
    }

    [RelayCommand]
    private async Task WriteRecord()
    {
        await Shell.Current.GoToAsync("CreateRecord");
    }

    [RelayCommand]
    private async Task DisplayRecords()
    {
        await Shell.Current.GoToAsync("DisplayRecords");
    }

    [RelayCommand]
    private async Task EditUsers()
    {
        await Shell.Current.GoToAsync("UserOverview");
    }
    
    [RelayCommand]
    private async Task EditBoats()
    {
        await Shell.Current.GoToAsync("BoatOverview");
    }

    [RelayCommand]
    private async Task SignIn()
    {
        await Shell.Current.GoToAsync("SignInForm");
    }

    public void RefreshSignValue()
    {
        IsUserSignedIn = _signedUserStorage.IsUserSigned;
    }
}
