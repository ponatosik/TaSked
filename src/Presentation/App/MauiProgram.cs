using Microsoft.Extensions.Logging;
using TaSked.Api.ApiClient;
using TaSked.App.Common;
using UraniumUI;

namespace TaSked.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseUraniumUI()
			.UseUraniumUIMaterial()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
		builder.Services.AddSingleton<IUserTokenStore, UserTokenSecureStorage>();

		builder.Services.AddTaSkedApi(opt => {
			opt.BaseUrl = "https://localhost:44321";
			opt.Timeout = TimeSpan.FromMinutes(3);
		});

		builder.Services.AddSingleton<LoginService>();

		builder.Services.AddSingleton<CreateGroupPage>();
		builder.Services.AddSingleton<CreateGroupViewModel>();

		builder.Services.AddSingleton<TasksPage>();
		builder.Services.AddTransient<TasksViewModel>();

		builder.Services.AddSingleton<JoinGroupPage>();
		builder.Services.AddSingleton<JoinGroupViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
