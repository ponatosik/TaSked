using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.App.Common;
using TaSked.Domain;
using System.Reactive;
using ReactiveUI;

namespace TaSked.App;

public partial class RoleViewModel : ReactiveObject, IActivatableViewModel
{
	private readonly ITaSkedService _api;
	private readonly LoginService _loginService;
	private ObservableCollection<User> _roles;
	
	public ViewModelActivator Activator { get; } = new();

	public ObservableCollection<User> Roles
	{
		get => _roles;
		set => this.RaiseAndSetIfChanged(ref _roles, value);
	}
	
	private bool _isRefreshing;
	public bool IsRefreshing
	{
		get => _isRefreshing;
		set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
	}

	public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

	public RoleViewModel(ITaSkedService api, LoginService loginService)
	{
		_api = api;
		_loginService = loginService;
		_roles = new ObservableCollection<User>();

		RefreshCommand = ReactiveCommand.CreateFromTask(async () =>
		{
			IsRefreshing = true;
			await ReloadRole();
			IsRefreshing = false;
		});
	}

	public async Task ReloadRole()
	{
		Roles.Clear();
		var currentGroupId = await _loginService.GetGroupIdAsync();
		if (currentGroupId.HasValue)
		{
			var roles = await LoadMembersAsync(currentGroupId.Value);
			foreach (var role in roles)
			{
				Roles.Add(role);
			}
		}
	}

	private async Task<List<User>> LoadMembersAsync(Guid groupId)
	{
		return await _api.GetGroupMembers(groupId);
	}
}