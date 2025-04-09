using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;
using BoatRecords.Models.Entities;

namespace BoatRecords.Pages;

[QueryProperty(nameof(BoatForEdit), "boat")]
internal partial class EditBoatViewModel : BaseViewModel
{
    private Boat? _boatForEdit;

    public Boat? BoatForEdit
    {
        get { return _boatForEdit; }
        set
        {
            _boatForEdit = value;
            OnPropertyChange(nameof(BoatForEdit));
            Name = value?.Name ?? "";
        }
    }

    private readonly BoatsStorage _boatsStorage;

    private string _name { get; set; } = "";

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChange(nameof(Name));
        }
    }

    public EditBoatViewModel(BoatsStorage boatsStorage)
    {
        _boatsStorage = boatsStorage;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task SubmitValues()
    {
        if (Name == "")
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Jméno musí být vyplněno!", "OK");
            return;
        }

        await _boatsStorage.UpdateBoat(Name, BoatForEdit);
        await Shell.Current.Navigation.PopAsync();
    }
}
