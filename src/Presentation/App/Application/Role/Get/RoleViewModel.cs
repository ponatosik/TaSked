using Api.Requests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.App.Common;
using TaSked.Domain;

namespace TaSked.App;

public partial class RoleViewModel : ObservableObject
{
	private readonly ITaSkedSevice _api;
    private LoginService _loginService;

    [ObservableProperty]
    private ObservableCollection<User> _roles;

    public RoleViewModel(ITaSkedSevice api, LoginService loginService)
	{
		_api = api;
		_roles = new ObservableCollection<User>();
        _loginService = loginService;
    }

    [RelayCommand]
    public async Task ReloadRole()
    {
        Roles.Clear();
        var currentGroupId = await _loginService.GetGroupIdAsync();
        var roles = LoadMembers(currentGroupId.Value);
        foreach (var role in roles)
        {
            Roles.Add(role);
        }
    }

    private List<User> LoadMembers(Guid groupId)
    {
        return _api.GetMembers(groupId).Result;
    }
}