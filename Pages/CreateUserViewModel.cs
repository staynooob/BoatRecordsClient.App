using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;

namespace BoatRecords.Pages;

internal partial class CreateUserViewModel : BaseViewModel
{
    private readonly UsersStorage _usersStorage;

    private string _name { get; set; } = "";
    private string _surname { get; set; } = "";
    private string _nickName { get; set; } = "";
    private string _gender { get; set; } = "";
    private DateTime _birthDate { get; set; } = new DateTime(2012, 1, 1);

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChange(nameof(Name));
        }
    }

    public string Surname
    {
        get { return _surname; }
        set
        {
            _surname = value;
            OnPropertyChange(nameof(Surname));
        }
    }

    public string NickName
    {
        get { return _nickName; }
        set
        {
            _nickName = value;
            OnPropertyChange(nameof(NickName));
        }
    }

    public string Gender
    {
        get { return _gender; }
        set
        {
            _gender = value;
            OnPropertyChange(nameof(Gender));
        }
    }

    public DateTime BirthDate
    {
        get { return _birthDate; }
        set
        {
            _birthDate = value;
            OnPropertyChange(nameof(BirthDate));
        }
    }

    public CreateUserViewModel(UsersStorage usersStorage)
    {
        _usersStorage = usersStorage;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task SubmitValues()
    {
        if (Name == "" || Surname == "" || Gender == "")
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Vyplňte všechna potřebná pole!", "OK");
            return;
        }

        await _usersStorage.CreateUser(
            Name,
            Surname,
            NickName,
            Gender,
            BirthDate
        );

        await Shell.Current.Navigation.PopAsync();
    }
}
