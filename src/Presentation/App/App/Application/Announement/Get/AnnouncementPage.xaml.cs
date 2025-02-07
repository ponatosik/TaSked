using ReactiveUI;
using ReactiveUI.Maui;

namespace TaSked.App;

public partial class AnnouncementPage : ReactiveContentPage<AnnouncementViewModel>
{
	public AnnouncementPage(AnnouncementViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		this.WhenActivated((_) => { });
	}
}