using System.Collections.ObjectModel;

namespace BoatRecords.Models.Entities;

class UserGroup : ObservableCollection<User>
{
    public string GroupName { get; private set; }

    public UserGroup(string groupName, ObservableCollection<User> users) : base(users)
    {
        GroupName = groupName;
    }
}
