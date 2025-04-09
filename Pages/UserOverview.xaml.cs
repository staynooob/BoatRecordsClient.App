namespace BoatRecords.Pages;

public partial class UserOverview : ContentPage
{
    public UserOverview(object bindingContext)
	{
		InitializeComponent();

        BindingContext = bindingContext;
    }
}
