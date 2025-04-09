namespace BoatRecords.Pages;

public partial class EditRecord : ContentPage
{
    public EditRecord(object bindingContext)
	{
		InitializeComponent();
        BindingContext = bindingContext;
    }
}
