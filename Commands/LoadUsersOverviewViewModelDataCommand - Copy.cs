using BoatRecords.Models.Entities;
using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using BoatRecords.Pages;

namespace BoatRecords.Commands;

class LoadUsersOverviewViewModelDataCommand : BaseAsyncCommand
{
    private readonly UserOverviewViewModel _viewModel;
    private readonly UsersStorage _usersStorage;

    public LoadUsersOverviewViewModelDataCommand(
        UserOverviewViewModel viewModel,
        UsersStorage usersStorage
    ) {
        _viewModel = viewModel;
        _usersStorage = usersStorage;
    }

    public override async Task ExecuteAsync(object param)
    {
        await _usersStorage.Load();
        _viewModel.LoadUsers(_usersStorage.Users);
    }
}
