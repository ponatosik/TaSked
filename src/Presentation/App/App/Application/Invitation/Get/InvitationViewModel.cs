using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;
using ReactiveUI;
using System.Reactive;


namespace TaSked.App;

public partial class InvitationViewModel : ReactiveObject
{
	private readonly ITaSkedService _api;
	private ObservableCollection<Invitation> _invitations;
	
	public ObservableCollection<Invitation> Invitations
	{
		get => _invitations;
		set => this.RaiseAndSetIfChanged(ref _invitations, value);
	}

	private bool _isRefreshing;
	public bool IsRefreshing
	{
		get => _isRefreshing;
		set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
	}

	public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

	public InvitationViewModel(ITaSkedService api)
	{
		_api = api;
		_invitations = new ObservableCollection<Invitation>();

		RefreshCommand = ReactiveCommand.CreateFromTask(async () =>
		{
			IsRefreshing = true;
			await ReloadInvitation();
			IsRefreshing = false;
		});
	}

	public async Task ReloadInvitation()
	{
		Invitations.Clear();
		var invitations = await LoadInvitations();
		foreach (var invitation in invitations)
		{
			Invitations.Add(invitation);
		}
	}

	private async Task<List<Invitation>> LoadInvitations()
	{
		var invitations = await _api.GetAllInvitation();
        
		if (invitations.Count == 0)
		{
			var invitation = await _api.CreateInvitation(new CreateInvitationRequest("defaultinvitation"));
			invitations.Add(invitation);
		}
        
		return invitations;
	}
}