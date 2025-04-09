using BoatRecords.Models.Entities;
using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using BoatRecords.Pages;

namespace BoatRecords.Commands;

class LoadViewRecordViewModelDataCommand : BaseAsyncCommand
{
    private readonly DisplayRecordsViewModel _viewModel;
    private readonly RecordsStorage _recordsStorage;

    public LoadViewRecordViewModelDataCommand(
        DisplayRecordsViewModel viewModel,
        RecordsStorage recordsStorage
    ) {
        _viewModel = viewModel;
        _recordsStorage = recordsStorage;
    }

    public override async Task ExecuteAsync(object param)
    {
        await _recordsStorage.Load();
        _viewModel.LoadRecords(_recordsStorage.Records);
    }
}
