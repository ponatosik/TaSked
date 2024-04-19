namespace TaSked.App;

public partial class AllTasksPage : ContentPage
{
	public AllTasksPage(AllTasksViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}