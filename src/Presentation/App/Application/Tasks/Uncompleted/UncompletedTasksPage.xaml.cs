using ReactiveUI;
using ReactiveUI.Maui;

namespace TaSked.App;

public partial class UncompletedTasksPage : ReactiveContentPage<UncompletedTasksViewModel>
{
    public UncompletedTasksPage(UncompletedTasksViewModel viewModel)
    { 
		InitializeComponent();
		ViewModel = viewModel;
		this.WhenActivated(_ => { });
    }
}