using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaSked.Api.ApiClient;

namespace TaSked.App;

public class CreateGroupViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	private string _groupName;
	private string _userNickname;

	private readonly ITaSkedSevice _api;
	private readonly IUserTokenStore _userTokenStore;

	public ICommand CreateGroupCommand { get; set; }

	public string GroupName
	{
		get => _groupName;
		set
		{
			if (_groupName != value)
			{
				_groupName = value;
				OnPropertyChanged();
			}
		}
	}

	public string UserNickname
	{
		get => _userNickname;
		set
		{
			if (_userNickname != value)
			{
				_userNickname = value;
				OnPropertyChanged();
			}
		}
	}

    public CreateGroupViewModel(ITaSkedSevice taSkedSevice, IUserTokenStore userTokenStore)
    {
		_api = taSkedSevice;
		_userTokenStore = userTokenStore;
		CreateGroupCommand = new Command(CreateGroup);
    }

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

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
