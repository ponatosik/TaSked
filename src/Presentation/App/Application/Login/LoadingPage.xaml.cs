using TaSked.App.Common;

namespace TaSked.App;

public partial class LoadingPage : ContentPage
{
    private readonly LoginService _loginService;
    public LoadingPage(LoginService loginService)
	{
		InitializeComponent();
        _loginService = loginService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (await _loginService.IsAuthorized())
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
        base.OnNavigatedTo(args);
    }
}