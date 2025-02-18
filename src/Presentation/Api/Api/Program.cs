using PushNotifications.Requests;
using TaSked.Api.Configuration;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;
using TaSked.Infrastructure.ExceptionHandling;
using TaSked.Infrastructure.Persistence;
using TaSked.Infrastructure.PushNotifications;
using TaSked.Infrastructure.PushNotifications.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var useNotifications = configuration["UseFirebaseCloudMessaging"]?.ToLower() == "true";
var firebaseCredentials = configuration["FIREBASE_ADMIN_CREDENTIALS"];
	
builder.Services.AddControllers();
builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssemblyContaining<CreateUserCommand>();

	if (useNotifications)
	{
		config.RegisterServicesFromAssemblyContaining<SubscribeUserToNotificationsCommand>();
	}
});

if (useNotifications)
{
	builder.Services.AddFirebaseNotifications(firebaseCredentials!);
}

builder.Services.AddPolicyBasedAuthorization();
builder.Services.AddPersistence(configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddControllerInputValidation();

builder.AddJwtAuthentication();


var app = builder.Build();


app.UseSwaggerConfiguration();

app.UseAuthentication();

app.UseAuthorization();

app.UseApplicationExceptionHandling();

app.UseDomainExceptionHandling();

app.MapControllers();

if (useNotifications)
{
	app.MapNotificationsEndpoints();
}


app.Run();
