using BoatRecords.Commands;
using BoatRecords.Models.Entities;
using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;

namespace BoatRecords.Pages;

internal partial class BoatOverviewViewModel : BaseViewModel
{
    private readonly BoatsStorage _boatsStorage;

    private ObservableCollection<BoatGroup> _boats = new ObservableCollection<BoatGroup>();
    private Boat? _selectedBoat { get; set; } = null;
    private bool _isItemSelected { get; set; } = false;
    public ICommand LoadBoatsViewModelDataCommand { get; }

    public ObservableCollection<BoatGroup> Boats => _boats;
    public Boat SelectedBoat
    {
        get { return _selectedBoat; }
        set
        {
            _selectedBoat = value;
            OnPropertyChange(nameof(SelectedBoat));
            IsItemSelected = _selectedBoat != null;
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

    public BoatOverviewViewModel(BoatsStorage boatsStorage)
    {
        LoadBoatsViewModelDataCommand = new LoadBoatsOverviewViewModelDataCommand(this, boatsStorage);
        _boatsStorage = boatsStorage;
        _boatsStorage.BoatCreated += OnBoatCreated;
        _boatsStorage.BoatsUpdated += OnBoatsUpdated;
    }

    public static BoatOverviewViewModel LoadViewModel(BoatsStorage boatsStorage)
    {
        BoatOverviewViewModel viewModel = new BoatOverviewViewModel(boatsStorage);
        viewModel.LoadBoatsViewModelDataCommand.Execute(null);
        return viewModel;
    }

    private void OnBoatCreated(Boat boat)
    {
        IEnumerable<BoatGroup> groups = _boats.Where(group => group.GroupName == boat.GetCategoryName());

        BoatGroup group = groups.Count() == 0
            ? new BoatGroup(boat.GetCategoryName(), new ObservableCollection<Boat>())
            : groups.First();

        group.Add(boat);

        if (groups.Count() == 0)
        {
            _boats.Add(group);
        }
    }

    private void OnBoatsUpdated()
    {
        _boats.Clear();
        LoadBoats(_boatsStorage.Boats);
    }

    [RelayCommand]
    private async Task AddBoat()
    {
        await Shell.Current.GoToAsync("CreateBoat");
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _boatsStorage.BoatCreated -= OnBoatCreated;
        _boatsStorage.BoatsUpdated -= OnBoatsUpdated;
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task SubmitValues()
    {
        Dictionary<string, object?> parametres = new Dictionary<string, object?>()
        {
            { "boat", _selectedBoat }
        };

        await Shell.Current.GoToAsync("EditBoat", parametres);
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

        _boatsStorage.DeleteBoat(_selectedBoat);
    }

    public void LoadBoats(IEnumerable<ICategorisableEntity> boats)
    {
        boats = boats.Where(boat => boat.GetCategoryName() != boat.ToString());

        foreach (Boat boat in boats)
        {
            IEnumerable<BoatGroup> groups = _boats.Where(group => group.GroupName == boat.GetCategoryName());

            BoatGroup group = groups.Count() == 0
                ? new BoatGroup(boat.GetCategoryName(), new ObservableCollection<Boat>())
                : groups.First();

            group.Add(boat);

            if (groups.Count() == 0)
            {
                _boats.Add(group);
            }
        }
    }
}
