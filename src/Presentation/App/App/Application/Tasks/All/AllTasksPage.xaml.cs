using ReactiveUI;
using ReactiveUI.Maui;

namespace TaSked.App;

public partial class AllTasksPage : ReactiveContentPage<AllTasksViewModel>
{
	public AllTasksPage(AllTasksViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		this.WhenActivated(_ => { });
    }
}