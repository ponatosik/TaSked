using CommunityToolkit.Mvvm.Messaging;
using LocalizationResourceManager.Maui;
using TaSked.App.Common;

namespace TaSked.App;

public partial class App : Microsoft.Maui.Controls.Application
{
	private readonly ILocalizationResourceManager _localizationResourceManager;
	public App(ILocalizationResourceManager localizationResourceManager)
	{
		InitializeComponent();
		Current.UserAppTheme = AppTheme.Dark;
		_localizationResourceManager = localizationResourceManager;
        WeakReferenceMessenger.Default.Register<InvitationItemMessage>(this, async (r, m) =>
        {
	        string getinvintationid = m.Value.ToString();
	        var _loginService = ServiceHelper.GetService<LoginService>();
	        var isAuthorized = await _loginService.HasGroupAsync();
	        Device.StartTimer(TimeSpan.FromSeconds(2), () =>
	        {
		        if (isAuthorized)
		        {
			        string title = _localizationResourceManager["Invitation_Alert_AlreadyLoggedIn_Title"];
			        string message = _localizationResourceManager["Invitation_Alert_AlreadyLoggedIn_Message"];
			        string cancel = _localizationResourceManager["General_Alert_OkButton"];
                    
			        AppShell.Current.DisplayAlert(title, message, cancel);
		        }
		        else
		        {
			        AppShell.Current.GoToAsync($"JoinGroupPage").ContinueWith(async _ =>
			        {
				        var joinGroupPage = AppShell.Current.Navigation.NavigationStack.OfType<JoinGroupPage>().LastOrDefault();
				        if (joinGroupPage != null && joinGroupPage.BindingContext is JoinGroupViewModel viewModel)
				        {
					        viewModel.GroupInvitationId = getinvintationid;
				        }
			        });
		        }
		        return false;
	        });
        });
		MainPage = new AppShell();
	}
}
