namespace BoatRecords.Pages;

public partial class BoatOverview : ContentPage
{
    public BoatOverview(object bindingContext)
	{
		InitializeComponent();

        BindingContext = bindingContext;
    }
}
