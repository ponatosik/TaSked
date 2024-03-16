namespace TaSked.App;

public partial class ReportPage : ContentPage
{
	public ReportPage(RepotrsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}