using BoatRecords.Models.Entities;
using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;

namespace BoatRecords.Pages;

[QueryProperty(nameof(UserForEdit), "user")]
internal partial class EditUserViewModel : BaseViewModel
{
    private User? _userForEdit;

    public User? UserForEdit
    {
        get { return _userForEdit; }
        set
        {
            _userForEdit = value;
            OnPropertyChange(nameof(UserForEdit));
            OnSetDefaults(UserForEdit);
        }
    }

    private readonly UsersStorage _usersStorage;

    private string _name { get; set; } = "";
    private string _surname { get; set; } = "";
    private string? _nickName { get; set; }

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

    public string? NickName
    {
        get { return _nickName; }
        set
        {
            _nickName = value;
            OnPropertyChange(nameof(NickName));
        }
    }

    public EditUserViewModel(UsersStorage usersStorage)
    {
        _usersStorage = usersStorage;
    }

    private void OnSetDefaults(User? user)
    {
        if (user == null)
        {
            return;
        }

        string[] name = user.Name.Split(' ');
        string surname = "";

        for(int i = 1; i < name.Length; i++)
        {
            surname += " " + name[i];
        }

        Name = name.First();
        Surname = surname;
        NickName = user.NickName;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task SubmitValues()
    {
        if (Name == "" || Surname == "")
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Vyplňte všechna potřebná pole!", "OK");
            return;
        }

        await _usersStorage.UpdateUser(
            Name,
            Surname,
            NickName,
            UserForEdit
        );

        await Shell.Current.Navigation.PopAsync();
    }
}
