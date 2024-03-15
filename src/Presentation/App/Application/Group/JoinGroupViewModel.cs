using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class JoinGroupViewModel : ObservableObject
{
	private readonly ITaSkedSevice _api;
	private readonly IUserTokenStore _userTokenStore;

	[ObservableProperty]
	private string _groupInvitationId;
	[ObservableProperty]
	private string _userNickname;

    public JoinGroupViewModel(ITaSkedSevice taSkedSevice, IUserTokenStore userTokenStore)
    {
		_api = taSkedSevice;
		_userTokenStore = userTokenStore;
    }

	[RelayCommand]
	public void JoinGroup()
	{ 
		if (string.IsNullOrEmpty(_groupInvitationId) 
			|| string.IsNullOrEmpty(_userNickname)
			|| Guid.TryParse(_groupInvitationId, out Guid invitationId))
		{
			return;
		}

		var groupId = _api.GetInvitationById(invitationId).Result.GroupId;

		string token = _api.RegisterAnonymous(new Api.Requests.CreateUserTokenRequest(_userNickname)).Result;
		_userTokenStore.AccessToken = token;
		_api.ActivateInvitation(new Api.Requests.ActivateInvintationRequest(invitationId, groupId)).Wait();
        
         Shell.Current.GoToAsync("//TasksPage");
    }
}
