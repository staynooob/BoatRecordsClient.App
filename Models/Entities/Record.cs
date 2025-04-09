using System.Collections.ObjectModel;

namespace BoatRecords.Models.Entities;

internal class Record
{
    public Boat Boat { get; set; }
    public List<User> Crew { get; set; } = new List<User>();
    public DateTime DateOfRide { get; set; }
    public string RideUnificator { get; set; }
    public int Distance { get; set; }
}
