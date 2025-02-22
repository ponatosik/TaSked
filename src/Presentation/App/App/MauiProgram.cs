using Auth0.OidcClient;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.App.Application;
using TaSked.App.Caching;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.App.Common.Notifications;
using TaSked.App.Resources.Localization;
using TaSked.Infrastructure.LocalPersistence;
using The49.Maui.ContextMenu;
using UraniumUI;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TaSked.App;

public static class MauiProgram
{
	private const bool UseFirebaseNotifications = false;
	
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitCore()
			.UseUraniumUI()
			.UseUraniumUIMaterial()
			.UseLocalizationResourceManager(config =>
			{
				config.RestoreLatestCulture(true);
				config.AddResource(LocalizationResources.ResourceManager);
				config.SupportNameWithDots();
			})
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFontAwesomeIconFonts();
			})
			.ConfigureEssentials(essentials =>
			{
				essentials.UseVersionTracking();
			});

		builder.UseContextMenu();

		if (UseFirebaseNotifications)
		{
			builder.RegisterFirebaseServices();
			builder.Services.AddSingleton<NotificationsService>();
		}

		builder.Services.AddSingleton<IVersionTracking>(VersionTracking.Default);
		
		builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
		builder.Services.AddSingleton<UserTokenSecureStorage>();
		builder.Services.AddSingleton<IUserTokenStore>(sp => sp.GetRequiredService<UserTokenSecureStorage>());
		builder.Services.AddSingleton<IRefreshTokenStore>(sp => sp.GetRequiredService<UserTokenSecureStorage>());

		builder.Services.AddSingleton<IAuthHandlerManager, AuthHandlerManager>();
		builder.Services.AddSingleton<AnonymousAuthorizationHandler>();
		builder.Services.AddSingleton<Auth0AuthHandler>();
		
		builder.Services.AddLocalPersistence(FileSystem.AppDataDirectory);
		builder.Services.AddSingleton<HomeworkTasksService>();

		builder.Services.AddTaSkedApi(opt => {
			opt.BaseUrl = "https://taskedapi.azurewebsites.net/";
			opt.Timeout = TimeSpan.FromMinutes(3);
			opt.UseNotifications = UseFirebaseNotifications;
		});

		builder.Services.AddSingleton<LoginService>();
		builder.Services.AddSingleton<CreateGroupPage>();
		builder.Services.AddSingleton<CreateGroupViewModel>();

		builder.Services.AddSingleton<HomeworkDataSource>();
		builder.Services.AddSingleton<UncompletedTasksPage>();
		builder.Services.AddSingleton<UncompletedTasksViewModel>();
		builder.Services.AddScoped<CreateTaskPage>();
		builder.Services.AddScoped<CreateTaskViewModel>();
        builder.Services.AddScoped<UpdateTaskPage>();
        builder.Services.AddScoped<UpdateTaskViewModel>();
        builder.Services.AddScoped<TasksDetailsPage>();
        builder.Services.AddScoped<TasksDetailsViewModel>();
        builder.Services.AddScoped<AllTasksPage>();
        builder.Services.AddScoped<AllTasksViewModel>();
        builder.Services.AddScoped<SortBySubjPage>();
        builder.Services.AddScoped<SortBySubjViewModel>();

        builder.Services.AddSingleton<AnnouncementViewModel>();
        builder.Services.AddSingleton<AnnouncementPage>();
        builder.Services.AddSingleton<AnnouncementDataSource>();
        builder.Services.AddScoped<CreateAnnouncementPage>();
        builder.Services.AddScoped<CreateAnnouncementViewModel>();

		builder.Services.AddSingleton<JoinGroupPage>();
		builder.Services.AddSingleton<JoinGroupViewModel>();

		builder.Services.AddSingleton<SubjectsViewModel>();
		builder.Services.AddSingleton<SubjectPage>();
		builder.Services.AddScoped<CreateSubjectPage>();
		builder.Services.AddScoped<CreateSubjectViewModel>();
		builder.Services.AddScoped<UpdateSubjectPage>();
		builder.Services.AddScoped<UpdateSubjectViewModel>();
		builder.Services.AddScoped<SubjectDetailsPage>();
		builder.Services.AddScoped<SubjectDetailsViewModel>();
		builder.Services.AddSingleton<SubjectDataSource>();

		builder.Services.AddSingleton<RoleViewModel>();
		builder.Services.AddSingleton<RolePage>();

        builder.Services.AddSingleton<InvitationViewModel>();
        builder.Services.AddSingleton<InvitationsPage>();
        
		builder.Services.AddSingleton<LoginPage>();

        builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<SettingsPage>();

		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddSingleton<SettingsPage>();

        builder.Services.AddSingleton<LoadingPage>();
		builder.Services.AddSingleton<PopUpPage>();
		builder.Services.AddSingleton<AppState>();

		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

		builder.Services.AddTaSkedCache();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageViewModel>();

		builder.Services.AddSingleton(new Auth0Client(new()
		{
			Domain = "tasked-app.eu.auth0.com",
			ClientId = "dpA38Qq0JOmU2uvilzl52OkXjqjFreTq",
			RedirectUri = "myapp://callback/",
			PostLogoutRedirectUri = "myapp://callback/",
			Scope = "openid profile"


		}));
		
		RxApp.DefaultExceptionHandler = new AppExceptionHandler();

		var app = builder.Build();
		ServiceHelper.Initialize(app.Services);
		return app;
	}
}


