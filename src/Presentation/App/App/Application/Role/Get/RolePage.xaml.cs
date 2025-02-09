using ReactiveUI;
using ReactiveUI.Maui;

namespace TaSked.App;

public partial class RolePage : ReactiveContentPage<RoleViewModel>
{
	public RolePage(RoleViewModel viewModel)
	{
		InitializeComponent();
		ViewModel = viewModel;
		this.WhenActivated(_ => { viewModel.ReloadRole(); });
	}
}