using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class CreateGroupViewModel : ObservableObject
{
	[ObservableProperty]
	private string _groupName;
	[ObservableProperty]
	private string _userNickname;

	private readonly ITaSkedSevice _api;
	private readonly IUserTokenStore _userTokenStore;


    public CreateGroupViewModel(ITaSkedSevice taSkedSevice, IUserTokenStore userTokenStore)
    {
		_api = taSkedSevice;
		_userTokenStore = userTokenStore;
    }

	[RelayCommand]
	public async void CreateGroup()
	{ 
		if (string.IsNullOrEmpty(_groupName) || string.IsNullOrEmpty(_userNickname))
		{
			return;
		}

		string token = await _api.RegisterAnonymous(new Api.Requests.CreateUserTokenRequest(_userNickname));
		_userTokenStore.AccessToken = token;
		await _api.CreateGroup(new Api.Requests.CreateGroupRequest(_groupName));

		// TODO: Go to group page
	}
}
