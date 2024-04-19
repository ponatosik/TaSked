namespace TaSked.App;

public partial class SortBySubjPage : ContentPage
{
	public SortBySubjPage(SortBySubjViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}