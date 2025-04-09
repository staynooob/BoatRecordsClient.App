using BoatRecords.Commands;
using BoatRecords.Models.Entities;
using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BoatRecords.Pages;

internal partial class DisplayRecordsViewModel : BaseViewModel
{
    private readonly RecordsStorage _recordsStorage;

    private DateTime _date { get; set; } = DateTime.Now;
    private readonly ObservableCollection<Record> _records = new ObservableCollection<Record>();
    private Record _record;
    private bool _isItemSeleced = false;

    public DateTime Date
    {
        get { return _date; }
        set
        {
            _date = value;
            OnPropertyChange(nameof(Date));
            OnDateChanged(_date);
        }
    }
    public ObservableCollection<Record> Records => _records;
    public Record Record
    {
        get { return _record; }
        set
        {
            _record = value;
            OnPropertyChange(nameof(Record));
            IsItemSelected = Record != null;
        }
    }
    public bool IsItemSelected
    {
        get { return _isItemSeleced; }
        set
        {
            _isItemSeleced = value;
            OnPropertyChange(nameof(IsItemSelected));
        }
    }

    public ICommand LoadViewRecordViewModelDataCommand { get; }

    public DisplayRecordsViewModel(RecordsStorage recordsStorage)
    {
        LoadViewRecordViewModelDataCommand = new LoadViewRecordViewModelDataCommand(this, recordsStorage);
        _recordsStorage = recordsStorage;
        _recordsStorage.RecordCreated += OnRecordCreated;
        _recordsStorage.RecordsChanged += OnRecordsChanged;
    }

    public static DisplayRecordsViewModel LoadViewModel(RecordsStorage recordsStorage)
    {
        DisplayRecordsViewModel viewModel = new DisplayRecordsViewModel(recordsStorage);
        viewModel.LoadViewRecordViewModelDataCommand.Execute(null);
        return viewModel;
    }

    private void OnRecordsChanged(DateTime time)
    {
        _records.Clear();

        foreach (Record record in _recordsStorage.Records)
        {
            _records.Add(record);
        }
    }

    private void OnRecordCreated(Record record)
    {
        _records.Add(record);
    }

    private async void OnDateChanged(DateTime date)
    {
        await _recordsStorage.ChangeDate(date);
    }

    public void LoadRecords(IEnumerable<Record> records)
    {
        foreach (Record record in records)
        {
            _records.Add(record);
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _recordsStorage.RecordCreated -= OnRecordCreated;
        _recordsStorage.RecordsChanged -= OnRecordsChanged;
        await Shell.Current.Navigation.PopAsync();
    }

    [RelayCommand]
    private async Task SubmitValues()
    {
        Dictionary<string, object?> parametres = new Dictionary<string, object?>()
        {
            { "record", Record }
        };

        await Shell.Current.GoToAsync("EditRecord", parametres);
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

        _recordsStorage.DeleteRecord(_record);
    }
}
