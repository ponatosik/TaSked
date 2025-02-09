using ReactiveUI;
using ReactiveUI.Maui;

namespace TaSked.App;

public partial class InvitationsPage : ReactiveContentPage<InvitationViewModel>
{
    public InvitationsPage(InvitationViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        this.WhenActivated(_ => { viewModel.ReloadInvitation(); });
    }
}