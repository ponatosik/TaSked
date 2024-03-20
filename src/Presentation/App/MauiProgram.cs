using Microsoft.Extensions.Logging;
using TaSked.Api.ApiClient;
using TaSked.App.Application;
using TaSked.App.Common;
using The49.Maui.ContextMenu;
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

		builder.UseContextMenu();
		
		builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
		builder.Services.AddSingleton<IUserTokenStore, UserTokenSecureStorage>();

		builder.Services.AddTaSkedApi(opt => {
			opt.BaseUrl = "https://taskedapi.azurewebsites.net/";
			opt.Timeout = TimeSpan.FromMinutes(3);
		});

		//SecureStorage.Default.Remove("TaSked.AccessToken");

		builder.Services.AddSingleton<LoginService>();

		builder.Services.AddSingleton<CreateGroupPage>();
		builder.Services.AddSingleton<CreateGroupViewModel>();

		builder.Services.AddSingleton<TasksPage>();
		builder.Services.AddTransient<TasksViewModel>();

		builder.Services.AddSingleton<RepotrsViewModel>();
		builder.Services.AddSingleton<ReportPage>();
		builder.Services.AddScoped<CreateReportPage>();
		builder.Services.AddScoped<CreateReportViewModel>();

		builder.Services.AddSingleton<JoinGroupPage>();
		builder.Services.AddSingleton<JoinGroupViewModel>();

		builder.Services.AddSingleton<SubjectsViewModel>();
		builder.Services.AddSingleton<SubjectPage>();
		builder.Services.AddScoped<CreateSubjectPage>();
		builder.Services.AddScoped<CreateSubjectViewModel>();
		builder.Services.AddScoped<UpdateSubjectPage>();
		builder.Services.AddScoped<UpdateSubjectViewModel>();

		builder.Services.AddSingleton<RoleViewModel>();
		builder.Services.AddSingleton<RolePage>();

		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<SettingsPage>();

		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddSingleton<LoadingPage>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();
		ServiceHelper.Initialize(app.Services);
		return app;
	}
}
