using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Domain;

namespace TaSked.App;

public partial class RoleViewModel : ObservableObject
{
	private readonly ITaSkedSevice _api;

	[ObservableProperty]
    private ObservableCollection<User> _roles;

    public RoleViewModel(ITaSkedSevice api)
	{
		_api = api;
		_roles = new ObservableCollection<User>();
	}

	[RelayCommand]
    public void ReloadRole()
    {
        _roles.Clear();
        _roles.ToList().ForEach(role => LoadMembers(role.Id).ForEach(member => _roles.Add(member)));
    }

    private List<User> LoadMembers(Guid groupId)
    {
        return _api.GetMembers(groupId).Result;
    }
}

