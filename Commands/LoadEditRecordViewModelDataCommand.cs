using BoatRecords.Models.Entities;
using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using BoatRecords.Pages;

namespace BoatRecords.Commands;

class LoadEditRecordViewModelDataCommand : BaseAsyncCommand
{
    private readonly EditRecordViewModel _viewModel;
    private readonly UsersStorage _usersStorage;
    private readonly BoatsStorage _boatsStorage;

    public LoadEditRecordViewModelDataCommand(
        EditRecordViewModel viewModel,
        UsersStorage usersStorage,
        BoatsStorage boatsStorage
    ) {
        _viewModel = viewModel;
        _usersStorage = usersStorage;
        _boatsStorage = boatsStorage;
    }

    public override async Task ExecuteAsync(object param)
    {
        await _usersStorage.Load();
        _viewModel.LoadUsers(_usersStorage.Users);

        await _boatsStorage.Load();
        _viewModel.LoadBoats(_boatsStorage.Boats);
    }
}
