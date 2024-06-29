using ReactiveUI;
using ReactiveUI.Maui;

namespace TaSked.App;

public partial class ReportPage : ReactiveContentPage<ReportsViewModel>
{
	public ReportPage(ReportsViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		this.WhenActivated((_) => { });
	}
}