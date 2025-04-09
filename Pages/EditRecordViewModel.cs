using BoatRecords.Commands;
using BoatRecords.ContentViews;
using BoatRecords.ContentViews.DoublePicker.EventArgsuments;
using BoatRecords.Models.Collections;
using BoatRecords.Models.Entities;
using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BoatRecords.Pages;


[QueryProperty(nameof(RecordForEdit), "record")]
internal partial class EditRecordViewModel : BaseViewModel
{
    private Record? _recordForEdit;

    public Record? RecordForEdit
    {
        get { return _recordForEdit; }
        set
        {
            _recordForEdit = value;
            OnPropertyChange(nameof(RecordForEdit));
            OnSetDefaults(RecordForEdit);
        }
    }

    private RecordsStorage _recordsStorage;

    private DictionarisedEntity _users;
    private DictionarisedEntity _boats;
    private DateTime _rideDate = DateTime.Today;
    private int _distance;
    private readonly ObservableCollection<DoublePickerViewModel> _crew = new ObservableCollection<DoublePickerViewModel>();
    private DoublePickerViewModel _boat;
    public ICommand LoadEditRecordViewModelDataCommand { get; }

    public ObservableCollection<DoublePickerViewModel> Crew => _crew;
    public DoublePickerViewModel Boat
    {
        get { return _boat; }
        set
        {
            _boat = value;
            OnPropertyChange(nameof(Boat));
        }
    }
    public DateTime RideDate
    {
        get { return _rideDate; }
        set
        {
            _rideDate = value;
            OnPropertyChange(nameof(RideDate));
        }
    }
    public int Distance {
        get { return _distance; }
        set
        {
            _distance = value;
            OnPropertyChange(nameof(Distance));
        }
    }

    public EditRecordViewModel(
        UsersStorage usersStorage,
        BoatsStorage boatsStorage,
        RecordsStorage recordsStorage
    ) {
        LoadEditRecordViewModelDataCommand = new LoadEditRecordViewModelDataCommand(this, usersStorage, boatsStorage);
        _recordsStorage = recordsStorage;
        SetupBoatCallback();
    }

    public static EditRecordViewModel LoadViewModel(
        UsersStorage usersStorage,
        BoatsStorage boatsStorage,
        RecordsStorage recordsStorage
    ) {
        EditRecordViewModel viewModel = new EditRecordViewModel(usersStorage, boatsStorage, recordsStorage);
        viewModel.LoadEditRecordViewModelDataCommand.Execute(null);
        return viewModel;
    }

    public void BoatSelected(object sender, EventArgs e)
    {
        _crew.Clear();
        var args = (e as SubCategoryEventArguments);
        var boatEntity = (args.CategoryEntity as Boat);

        var num = boatEntity.SeatsNumber;

        for (int i = 0; i < num; i++) //BOAT CREW
        {
            var dbvm = new DoublePickerViewModel();
            dbvm.SetCategorisedItems(_users);
            dbvm.CategoryTitle = "Kategorie";
            dbvm.SubCategoryTitle = "Veslař";
            _crew.Add(dbvm);
        }

        if (!boatEntity.IsCoxed)
        {
            return;
        }

        var dbvimo = new DoublePickerViewModel(); //COX
        dbvimo.SetCategorisedItems(_users);
        dbvimo.CategoryTitle = "Kategorie";
        dbvimo.SubCategoryTitle = "Kormidelník";
        _crew.Add(dbvimo);
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task SubmitValues()
    {
        var users = new List<User>();

        if (_crew.Count() == 0)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Musí být zvolena loď.", "OK");
            return;
        }

        foreach (DoublePickerViewModel entity in _crew)
        {
            if (entity.SelectedCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Musí být vyplněn každý člen posádky.", "OK");
                return;
            }

            User u = (entity.SubCategories.ElementAt(entity.SelectedSubCategoryIndex) as User);

            if (users.IndexOf(u) != -1)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Každý člen posádky musí být unikátní.", "OK");
                return;
            }

            users.Add(u);
        }

        try
        {
            await _recordsStorage.ChangeRecord(
                users,
                Distance,
                DateOnly.FromDateTime(RideDate),
                (Boat.SubCategories.ElementAt(Boat.SelectedSubCategoryIndex) as Boat),
                RecordForEdit
            );
            await Application.Current.MainPage.DisplayAlert("Success", "Záznam byl úspěšně upraven.", "OK");
            await Shell.Current.Navigation.PopAsync();
        } catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Při přidávání došlo k chybě. Zkuste to prosím později.", "OK");
        }
    }

    public void LoadUsers(IEnumerable<ICategorisableEntity> users)
    {
        _users = new DictionarisedEntity(users);
    }

    public void LoadBoats(IEnumerable<ICategorisableEntity> boats)
    {
        _boats = new DictionarisedEntity(boats);
        _boat.SetCategorisedItems(_boats);
    }

    private void SetupBoatCallback()
    {
        _boat = new DoublePickerViewModel();
        _boat.CategoryTitle = "Druh Lodi";
        _boat.SubCategoryTitle = "Jméno Lodi";
        _boat.OnSuccess += BoatSelected;
    }

    private void OnSetDefaults(Record? record)
    {
        if (record == null)
        {
            return;
        }

        RideDate = record.DateOfRide;
        Distance = record.Distance;

        string? selectedBoatCategory = Boat.Categories
            .Where(category => category == record.Boat.GetCategoryName())
            .First();

        if (selectedBoatCategory != null)
        {
            Boat.SelectedCategory = selectedBoatCategory;
        }

        var selectedSubCategory = Boat.SubCategories
            .Where(subcategory => subcategory.ToString() == record.Boat.Name)
            .First();

        if (selectedSubCategory != null)
        {
            Boat.SelectedSubCategoryIndex = Boat.SubCategories.IndexOf(selectedSubCategory);
        }

        foreach (var crewMember in Crew)
        {
            User correspondingCrewMember = record.Crew.ElementAt(Crew.IndexOf(crewMember));

            var selectedMemberCategory = crewMember.Categories
                .Where(category => category == correspondingCrewMember.GetCategoryName())
                .First();

            if (selectedMemberCategory != null)
            {
                crewMember.SelectedCategory = selectedMemberCategory;
                crewMember.ChangeCategoryCommand.Execute(null);
            }

            var selectedMemberSubCategory = crewMember.SubCategories
                .Where(subcategory => subcategory.ToString() == correspondingCrewMember.ToString())
                .First();

            if (selectedMemberSubCategory != null)
            {
                crewMember.SelectedSubCategoryIndex = crewMember.SubCategories.IndexOf(selectedMemberSubCategory);
            }
        }
    }
}
