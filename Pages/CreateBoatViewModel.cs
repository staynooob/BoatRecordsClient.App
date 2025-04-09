using BoatRecords.Models.Enums;
using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BoatRecords.Models.Helpers;

namespace BoatRecords.Pages;

internal partial class CreateBoatViewModel : BaseViewModel
{
    private readonly BoatsStorage _boatsStorage;

    private ObservableCollection<string> _categories = new ObservableCollection<string>();
    private string _category { get; set; } = "";
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
    public string Category
    {
        get { return _category; }
        set
        {
            _category = value;
            OnPropertyChange(nameof(Category));
        }
    }
    public ObservableCollection<string> Categories => _categories;

    public CreateBoatViewModel(BoatsStorage boatsStorage)
    {
        foreach(BoatType type in Enum.GetValues(typeof(BoatType)))
        {
            _categories.Add(EnumHelpers.ToStringEnums(type));
        }

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
            await Application.Current.MainPage.DisplayAlert("Error", "Vyplňte všechna potřebná pole!", "OK");
            return;
        }

        BoatType selectedCategory = BoatType.SingleScull;

        foreach (BoatType type in Enum.GetValues(typeof(BoatType)))
        {
            if(EnumHelpers.ToStringEnums(type) == Category)
            {
                selectedCategory = type;
                break;
            }
        }

        await _boatsStorage.CreateBoat(
            Name,
            selectedCategory
        );

        await Shell.Current.Navigation.PopAsync();
    }
}
