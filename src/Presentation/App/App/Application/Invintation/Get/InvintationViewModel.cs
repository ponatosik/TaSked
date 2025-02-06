using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App;

public partial class InvintationViewModel : ObservableObject
{
	private readonly ITaSkedService _api;

    [ObservableProperty]
    private ObservableCollection<Invitation> _invintations;

    public InvintationViewModel(ITaSkedService api)
	{
		_api = api;
        _invintations = new ObservableCollection<Invitation>();
    }

    [RelayCommand]
    public void ReloadInvintation()
    {
        Invintations.Clear();
        LoadInvintations().ForEach(invintation => Invintations.Add(invintation));

    }

    private List<Invitation> LoadInvintations()
    {
        List<Invitation> invintations = _api.GetAllInvitation().Result;

        if (invintations.Count == 0)
        {
	        var invintation = _api.CreateInvitation(new CreateInvitationRequest("defaultinvintation")).Result;
            invintations.Add(invintation);
        }

        return invintations;
    }
}