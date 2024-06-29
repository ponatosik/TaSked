using ReactiveUI;
using ReactiveUI.Maui;

namespace TaSked.App;

public partial class SubjectPage : ReactiveContentPage<SubjectsViewModel>
{
	public SubjectPage(SubjectsViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		this.WhenActivated(_ => { });
	}
}