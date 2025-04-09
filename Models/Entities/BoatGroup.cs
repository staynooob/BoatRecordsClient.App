using System.Collections.ObjectModel;

namespace BoatRecords.Models.Entities;

class BoatGroup : ObservableCollection<Boat>
{
    public string GroupName { get; private set; }

    public BoatGroup(string groupName, ObservableCollection<Boat> boats) : base(boats)
    {
        GroupName = groupName;
    }
}
