using BoatRecords.Commands;
using BoatRecords.Models.Services;
using BoatRecords.Models.Storages;
using BoatRecords.Pages;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BoatRecords
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("StudioSans Bold.otf", "StudioSansBold");
                    fonts.AddFont("StudioSans Extra Bold.otf", "StudioSansExtraBold");
                    fonts.AddFont("StudioSans Extra Light.otf", "StudioSansExtraLight");
                    fonts.AddFont("StudioSans Light.otf", "StudioSansLight");
                    fonts.AddFont("StudioSans Medium.otf", "StudioSansMedium");
                    fonts.AddFont("StudioSans.otf", "StudioSans");
                });

            builder.Services.AddSingleton<UsersRequests>();
            builder.Services.AddSingleton<RecordsRequests>();
            builder.Services.AddSingleton<BoatsRequests>();

            builder.Services.AddTransient<MainMenuViewModel>();
            builder.Services.AddTransient(
                s => new MainMenu(s.GetRequiredService<MainMenuViewModel>())
            );

            builder.Services.AddTransient<EditRecordViewModel>();
            builder.Services.AddTransient<EditRecord>(
                s => new EditRecord(
                    EditRecordViewModel.LoadViewModel(
                        s.GetRequiredService<UsersStorage>(),
                        s.GetRequiredService<BoatsStorage>(),
                        s.GetRequiredService<RecordsStorage>()
                    )
                )
            );

            builder.Services.AddTransient<CreateRecordViewModel>();
            builder.Services.AddTransient<CreateRecord>(
                s => new CreateRecord(
                    CreateRecordViewModel.LoadViewModel(
                        s.GetRequiredService<UsersStorage>(),
                        s.GetRequiredService<BoatsStorage>(),
                        s.GetRequiredService<RecordsStorage>()

                    )
                )
            );

            builder.Services.AddTransient<SignInFormViewModel>();
            builder.Services.AddTransient<SignInForm>(
                s => new SignInForm(s.GetRequiredService<SignInFormViewModel>())
            );

            builder.Services.AddTransient<EditUserViewModel>();
            builder.Services.AddTransient<EditUser>(
                s => new EditUser(s.GetRequiredService<EditUserViewModel>())
            );

            builder.Services.AddTransient<CreateUserViewModel>();
            builder.Services.AddTransient<CreateUser>(
                s => new CreateUser(s.GetRequiredService<CreateUserViewModel>())
            );

            builder.Services.AddTransient<EditBoatViewModel>();
            builder.Services.AddTransient<EditBoat>(
                s => new EditBoat(s.GetRequiredService<EditBoatViewModel>())
            );

            builder.Services.AddTransient<CreateBoatViewModel>();
            builder.Services.AddTransient<CreateBoat>(
                s => new CreateBoat(s.GetRequiredService<CreateBoatViewModel>())
            );

            builder.Services.AddTransient<DisplayRecordsViewModel>();
            builder.Services.AddTransient<DisplayRecords>(
                s => new DisplayRecords(
                    DisplayRecordsViewModel.LoadViewModel(s.GetRequiredService<RecordsStorage>())
                )
            );

            builder.Services.AddTransient<UserOverviewViewModel>();
            builder.Services.AddTransient<UserOverview>(
                s => new UserOverview(
                    UserOverviewViewModel.LoadViewModel(s.GetRequiredService<UsersStorage>())
                )
            );

            builder.Services.AddTransient<BoatOverviewViewModel>();
            builder.Services.AddTransient<BoatOverview>(
                s => new BoatOverview(
                    BoatOverviewViewModel.LoadViewModel(s.GetRequiredService<BoatsStorage>())
                )
            );

            builder.Services.AddSingleton<SignedUserStorage>();
            builder.Services.AddSingleton<UsersStorage>();
            builder.Services.AddSingleton<BoatsStorage>();
            builder.Services.AddSingleton<RecordsStorage>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
