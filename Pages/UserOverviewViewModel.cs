using BoatRecords.Commands;
using BoatRecords.Models.Entities;
using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BoatRecords.Pages;

internal partial class UserOverviewViewModel : BaseViewModel
{
    private readonly UsersStorage _usersStorage;

    private ObservableCollection<UserGroup> _users = new ObservableCollection<UserGroup>();
    private User? _selectedUser;
    private bool _isItemSelected { get; set; } = false;
    public ICommand LoadUsersViewModelDataCommand { get; }

    public ObservableCollection<UserGroup> Users => _users;
    public User? SelectedUser
    {
        get { return _selectedUser; }
        set
        {
            _selectedUser = value;
            OnPropertyChange(nameof(SelectedUser));
            IsItemSelected = _selectedUser != null;
        }
    }
    public bool IsItemSelected
    {
        get { return _isItemSelected; }
        set
        {
            _isItemSelected = value;
            OnPropertyChange(nameof(IsItemSelected));
        }
    }

    public UserOverviewViewModel(UsersStorage usersStorage)
    {
        LoadUsersViewModelDataCommand = new LoadUsersOverviewViewModelDataCommand(this, usersStorage);
        _usersStorage = usersStorage;
        _usersStorage.UserCreated += OnUserCreated;
        _usersStorage.UsersUpdated += OnUsersUpdated;
    }

    public static UserOverviewViewModel LoadViewModel(UsersStorage usersStorage)
    {
        UserOverviewViewModel viewModel = new UserOverviewViewModel(usersStorage);
        viewModel.LoadUsersViewModelDataCommand.Execute(null);
        return viewModel;
    }

    private void OnUserCreated(User user)
    {
        IEnumerable<UserGroup> groups = _users.Where(group => group.GroupName == user.GetCategoryName());

        UserGroup group = groups.Count() == 0
            ? new UserGroup(user.GetCategoryName(), new ObservableCollection<User>())
            : groups.First();

        group.Add(user);

        if (groups.Count() == 0)
        {
            _users.Add(group);
        }
    }

    private void OnUsersUpdated()
    {
        _users.Clear();
        LoadUsers(_usersStorage.Users);
    }

    [RelayCommand]
    private async Task AddUser()
    {
        await Shell.Current.GoToAsync("CreateUser");
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _usersStorage.UserCreated -= OnUserCreated;
        _usersStorage.UsersUpdated -= OnUsersUpdated;
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task SubmitValues()
    {
        Dictionary<string, object?> parametres = new Dictionary<string, object?>()
        {
            { "user", _selectedUser }
        };

        await Shell.Current.GoToAsync("EditUser", parametres);
    }

    [RelayCommand]
    private async Task DeleteValues()
    {

        bool response = await Application
            .Current
            .MainPage
            .DisplayAlert("Warning", "Opravdu chcete smazat tento záznam?", "OK", "Cancel");

        if (response != true)
        {
            return;
        }

        _usersStorage.DeleteUser(SelectedUser);
    }

    public void LoadUsers(IEnumerable<ICategorisableEntity> users)
    {
        foreach (User user in users)
        {
            IEnumerable<UserGroup> groups = _users.Where(group => group.GroupName == user.GetCategoryName());

            UserGroup group = groups.Count() == 0
                ? new UserGroup(user.GetCategoryName(), new ObservableCollection<User>())
                : groups.First();

            group.Add(user);

            if (groups.Count() == 0)
            {
                _users.Add(group);
            }
        }
    }
}
