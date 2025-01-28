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
		// Win UI dotnet Maui bug: https://learn.microsoft.com/en-us/answers/questions/1375393/pending-navigations-still-processing-when-navigati
		await Task.Delay(1);

		if (await _loginService.HasGroupAsync())
        {
            await Shell.Current.GoToAsync("//UncompletedTasksPage");
        }
        else
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        base.OnNavigatedTo(args);
    }
}