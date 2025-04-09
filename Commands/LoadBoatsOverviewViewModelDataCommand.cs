using BoatRecords.Models.Storages;
using BoatRecords.Pages;

namespace BoatRecords.Commands;

class LoadBoatsOverviewViewModelDataCommand : BaseAsyncCommand
{
    private readonly BoatOverviewViewModel _viewModel;
    private readonly BoatsStorage _boatsStorage;

    public LoadBoatsOverviewViewModelDataCommand(
        BoatOverviewViewModel viewModel,
        BoatsStorage boatsStorage
    ) {
        _viewModel = viewModel;
        _boatsStorage = boatsStorage;
    }

    public override async Task ExecuteAsync(object param)
    {
        await _boatsStorage.Load();
        _viewModel.LoadBoats(_boatsStorage.Boats);
    }
}
