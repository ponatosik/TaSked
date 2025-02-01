using Refit;
using TaSked.App.Application;
using TaSked.App.Common;

namespace TaSked.App;

public partial class MainPage : ContentPage
{
	private readonly LoginService _loginService;
	private readonly MainPageViewModel _viewModel;
	
	public MainPage(LoginService loginService, MainPageViewModel viewModel)
	{
		_loginService = loginService;
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

    private void JoinGroupTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("JoinGroupPage");
    }

    private void CreateGroupTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("CreateGroupPage");
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
	    await _viewModel.LoadUserNicknameAsync();
    }
}
