using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using BoatRecords.Pages;

namespace BoatRecords.Commands;

class SignInFormViewModelCommand : BaseAsyncCommand
{
    private SignInFormViewModel _signInFormViewModel;
    private SignedUserStorage _signedUserStorage;

    public SignInFormViewModelCommand(
        SignInFormViewModel signInFormViewModel,
        SignedUserStorage signedUserStorage
    ) {
        _signInFormViewModel = signInFormViewModel;
        _signedUserStorage = signedUserStorage;
    }

    public override async Task ExecuteAsync(object param)
    {
        bool isUseSignedIn = await UsersRequests.SignInUser(_signInFormViewModel.Username, _signInFormViewModel.Password);

        _signedUserStorage.IsUserSigned = isUseSignedIn;

        if (isUseSignedIn)
        {
            await Application.Current.MainPage.DisplayAlert("Success", "Successfully signed in", "OK");
        } else { 
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to sign in", "OK");
        }

        await Shell.Current.GoToAsync("//MainMenu");
    }
}
