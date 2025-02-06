using Api.Requests;
using TaSked.Api.ApiClient;
using TaSked.App.Common;
using TaSked.Domain;

namespace TaSked.App.Components;

public partial class RoleCard : ContentView
{
    public LoginService _loginService;
    public ITaSkedService _api;

	public static readonly BindableProperty RoleModelProperty =
		BindableProperty.Create(nameof(RoleModel), typeof(User), typeof(RoleCard));

	public User RoleModel
	{
		get => (User)GetValue(RoleModelProperty);
	}

	public RoleCard()
	{
        _loginService = ServiceHelper.GetService<LoginService>();
        _api = ServiceHelper.GetService<ITaSkedService>();
		InitializeComponent();
	}

    public async void PromoteMember()
    {
        var currentGroupId = await _loginService.GetGroupIdAsync();
        var request = new PromoteMemberRequest(RoleModel.Id);
        await _api.PromoteGroupMember(request, currentGroupId.Value);
    }

    public async void BanMember()
    {
        var currentGroupId = await _loginService.GetGroupIdAsync();
        var request = new BanMemberRequest(RoleModel.Id);
        await _api.BanGroupMember(request, currentGroupId.Value);
    }

    private void Promote_Clicked(object sender, EventArgs e)
    {
        PromoteMember();
    }

    private void Ban_Clicked(object sender, EventArgs e)
    {
        BanMember();
    }
}