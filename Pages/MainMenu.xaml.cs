namespace BoatRecords.Pages;

public partial class MainMenu : ContentPage
{
    public MainMenu(object bindingContext)
    {
        InitializeComponent();

        BindingContext = bindingContext;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MainMenuViewModel vm)
        {
            vm.RefreshSignValue();
        }
    }
}
