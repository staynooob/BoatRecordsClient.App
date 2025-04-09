using BoatRecords.Models.Entities;
using BoatRecords.Models.Enums;
using BoatRecords.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoatRecords.Models.Storages;

class UsersStorage
{
    private readonly List<User> _users;
    private readonly Lazy<Task> _initializeLazy;

    public event Action<User> UserCreated;
    public event Action UsersUpdated;

    public IEnumerable<User> Users => _users;

    public UsersStorage()
    {
        _users = new List<User>();
        _initializeLazy = new Lazy<Task>(Initialize);
    }

    public async Task Load()
    {
        await _initializeLazy.Value;
    }

    public async Task CreateUser(
        string name,
        string surname,
        string? nickName,
        string gender,
        DateTime birthDate
    ) {
        int newUserId = await UsersRequests.InsertNewRecord(
            name,
            surname,
            nickName,
            gender,
            DateOnly.FromDateTime(birthDate)
        );

        var user = new User()
        {
            Id = newUserId,
            BirthDate = DateOnly.FromDateTime(birthDate),
            Name = name + " " + surname,
            NickName = nickName,
            Gender = gender == "Female" ? Gender.Female : Gender.Male
        };

        _users.Add(user);
        OnUserCreated(user);
    }

    internal async Task UpdateUser(
        string name,
        string surname,
        string? nickName,
        User? user
    ) {
        if (user == null)
        {
            return;
        }

        await UsersRequests.UpdateUser(name, surname, nickName, user);

        user.Name = name + " " + surname;
        user.NickName = nickName;
    }

    public async void DeleteUser(User? user)
    {
        if (user == null)
        {
            return;
        }

        await UsersRequests.DeleteRecord(user);
        _users.Remove(user);
        OnUsersUpdated();
    }

    private void OnUserCreated(User user)
    {
        UserCreated?.Invoke(user);
    }

    private void OnUsersUpdated()
    {
        UsersUpdated?.Invoke();
    }

    private async Task Initialize()
    {
        var users = await UsersRequests.GatAllUsers();
        _users.Clear();

        foreach (User user in users)
        {
            _users.Add(user);
        }
    }
}
