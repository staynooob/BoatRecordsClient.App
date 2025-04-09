namespace BoatRecords.Pages;

public partial class SignInForm : ContentPage
{
	public SignInForm(object bindingContext)
	{
		InitializeComponent();

		BindingContext = bindingContext;
	}
}