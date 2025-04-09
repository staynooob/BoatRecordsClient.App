using BoatRecords.Pages;

namespace BoatRecords;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("MainMenu/CreateRecord", typeof(CreateRecord));
        Routing.RegisterRoute("MainMenu/DisplayRecords", typeof(DisplayRecords));
        Routing.RegisterRoute("MainMenu/DisplayRecords/EditRecord", typeof(EditRecord));
        Routing.RegisterRoute("MainMenu/SignInForm", typeof(SignInForm));
        Routing.RegisterRoute("MainMenu/UserOverview", typeof(UserOverview));
        Routing.RegisterRoute("MainMenu/UserOverview/CreateUser", typeof(CreateUser));
        Routing.RegisterRoute("MainMenu/UserOverview/EditUser", typeof(EditUser));
        Routing.RegisterRoute("MainMenu/BoatOverview", typeof(BoatOverview));
        Routing.RegisterRoute("MainMenu/BoatOverview/CreateBoat", typeof(CreateBoat));
        Routing.RegisterRoute("MainMenu/BoatOverview/EditBoat", typeof(EditBoat));
    }
}
