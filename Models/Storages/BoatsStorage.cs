using BoatRecords.Models.Entities;
using BoatRecords.Models.Enums;
using BoatRecords.Models.Services;

namespace BoatRecords.Models.Storages;

class BoatsStorage
{
    private readonly List<Boat> _boats;
    private readonly Lazy<Task> _initializeLazy;

    public event Action<Boat> BoatCreated;
    public event Action BoatsUpdated;

    public IEnumerable<Boat> Boats => _boats;

    public BoatsStorage()
    {
        _boats = new List<Boat>();
        _initializeLazy = new Lazy<Task>(Initialize);
    }

    public async Task Load()
    {
        await _initializeLazy.Value;
    }

    public async Task CreateBoat(
        string name,
        BoatType boatType
    ) {
        int seatsNumber = boatType switch
        {
            BoatType.SingleScull => 1,
            BoatType.Scull => 1,
            BoatType.DoubleScull => 2,
            BoatType.Pair => 2,
            BoatType.CoxedPair => 2,
            BoatType.Four => 4,
            BoatType.QuadrapleScull => 4,
            BoatType.CoxedFour => 4,
            BoatType.CoxedQuadrapleScull => 4,
            BoatType.Eight => 8,
            BoatType.PairedEight => 8,
        };

        bool isCoxed = boatType switch
        {
            BoatType.SingleScull => false,
            BoatType.Scull => true,
            BoatType.DoubleScull => false,
            BoatType.Pair => false,
            BoatType.CoxedPair => true,
            BoatType.Four => false,
            BoatType.QuadrapleScull => false,
            BoatType.CoxedFour => true,
            BoatType.CoxedQuadrapleScull => true,
            BoatType.Eight => true,
            BoatType.PairedEight => true,
        };

        bool isPaired = boatType switch
        {
            BoatType.SingleScull => true,
            BoatType.Scull => true,
            BoatType.DoubleScull => true,
            BoatType.Pair => false,
            BoatType.CoxedPair => false,
            BoatType.Four => false,
            BoatType.QuadrapleScull => true,
            BoatType.CoxedFour => false,
            BoatType.CoxedQuadrapleScull => true,
            BoatType.Eight => false,
            BoatType.PairedEight => true,
        };

        int newBoatId = await BoatsRequests
            .InsertNewRecord(name, seatsNumber, isCoxed, isPaired);

        var boat = new Boat()
        {
            Id = newBoatId,
            SeatsNumber = seatsNumber,
            IsCoxed = isCoxed,
            Paired = isPaired,
            Name = name
        };

        _boats.Add(boat);
        OnBoatCreated(boat);
    }

    public async Task UpdateBoat(string name, Boat? boat)
    {
        if (boat == null)
        {
            return;
        }

        await BoatsRequests.UpdateRecord(boat, name);
        boat.Name = name;
    }

    public async Task DeleteBoat(Boat? boat)
    {
        if (boat == null)
        {
            return;
        }

        await BoatsRequests.DeleteRecord(boat);
        _boats.Remove(boat);
        OnBoatsUpdated();
    }

    private void OnBoatCreated(Boat boat)
    {
        BoatCreated?.Invoke(boat);
    }

    private void OnBoatsUpdated()
    {
        BoatsUpdated?.Invoke();
    }

    private async Task Initialize()
    {
        var boats = await BoatsRequests.GatAllBoats();
        _boats.Clear();

        foreach (Boat boat in boats)
        {
            _boats.Add(boat);
        }
    }
}
