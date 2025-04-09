using BoatRecords.Models.Entities;
using BoatRecords.Models.Services;

namespace BoatRecords.Models.Storages;

class RecordsStorage
{
    private readonly List<Record> _records;
    private readonly Lazy<Task> _initializeLazy;

    public event Action<Record> RecordCreated;
    public event Action<DateTime> RecordsChanged;

    public IEnumerable<Record> Records => _records;

    public RecordsStorage()
    {
        _records = new List<Record>();
        _initializeLazy = new Lazy<Task>(Initialize);
    }

    public async Task Load()
    {
        await _initializeLazy.Value;
    }

    public async Task ChangeDate(DateTime date)
    {
        var records = await RecordsRequests.GatAllRecords(date);
        _records.Clear();

        foreach (Record record in records)
        {
            _records.Add(record);
        }
        OnRecordsChanged(date);
    }

    public async Task CreateRecord(
        List<User> users,
        int distance,
        DateOnly date,
        Boat boat
    ) {
        string unificator = await RecordsRequests.InsertNewRecord(users, distance, date, boat);

        var record = new Record()
        {
            RideUnificator = unificator,
            DateOfRide = date.ToDateTime(new TimeOnly()),
            Distance = distance,
            Boat = boat,
            Crew = users
        };

        _records.Add(record);
        OnRecordCreated(record);
    }

    public async Task ChangeRecord(
        List<User> users,
        int distance,
        DateOnly date,
        Boat boat,
        Record? record
    ) {
        if (record == null)
        {
            return;
        }

        string newUnificator = await RecordsRequests.ChangeRecord(users, distance, date, boat, record);

        record.Crew.Clear();
        record.Crew.AddRange(users);
        record.Distance = distance;
        record.DateOfRide = date.ToDateTime(TimeOnly.MinValue);
        record.Boat = boat;
        record.RideUnificator = newUnificator;
    }

    public async Task DeleteRecord(Record? record)
    {
        if (record == null)
        {
            return;
        }

        await RecordsRequests.DeleteRecord(record);
        _records.Remove(record);
        OnRecordsChanged(record.DateOfRide);
    }

    private void OnRecordCreated(Record record)
    {
        RecordCreated?.Invoke(record);
    }

    private void OnRecordsChanged(DateTime date)
    {
        RecordsChanged?.Invoke(date);
    }

    private async Task Initialize()
    {
        var records = await RecordsRequests.GatAllRecords(DateTime.Now);
        _records.Clear();

        foreach (Record record in records)
        {
            _records.Add(record);
        }
    }
}
