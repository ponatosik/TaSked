using ReactiveUI;
using ReactiveUI.Maui;

namespace TaSked.App;

public partial class SortBySubjPage : ReactiveContentPage<SortBySubjViewModel>
{
	public SortBySubjPage(SortBySubjViewModel viewModel)
	{
		InitializeComponent();
        ViewModel = viewModel;
		this.WhenActivated((_) => { });
    }
}