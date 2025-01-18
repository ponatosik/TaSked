using Google.Apis.Auth.OAuth2;
using PushNotifications.Requests;
using TaSked.Api.Configuration;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;
using TaSked.Infrastructure.ExceptionHandling;
using TaSked.Infrastructure.Persistance;
using TaSked.Infrastructure.Persistance.AzureMySqlInApp;
using TaSked.Infrastructure.PushNotifications;

var builder = WebApplication.CreateBuilder(args);


var useAzureMySqlInApp = builder.WebHost.GetSetting("UseAzureMySqlInApp")?.ToLower() == "true";
var jwtSecretKey = builder.WebHost.GetSetting("JwtSecretKey");
var baseUrls = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey) ?? "http://localhost:5070";
var baseUrlList = baseUrls.Split(';');
var googleCredential = GoogleCredential.FromJson(builder.WebHost.GetSetting("FIREBASE_ADMIN_CREDENTIALS"));

if (jwtSecretKey is null && builder.Environment.IsDevelopment())
{
	jwtSecretKey = "{774F9515-F749-42F1-8578-8BA810C3BA78}";
}

builder.Services.AddControllers();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(
	typeof(CreateHomeworkCommand).Assembly,
	typeof(SubscribeUserToNotificationsCommand).Assembly)
);

builder.Services.AddFirebaseNotifications(googleCredential);
builder.Services.AddPolicyBasedAuthorization();
builder.Services.AddPersistance(useAzureMySqlInApp ? opt => opt.UseAzureMysqlInApp() : null);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddJwtAuthentication(new JwtOptions
{
	Issuer = baseUrlList.First(),
	Audience = baseUrlList.First(),
	SecretKey = jwtSecretKey!
});


var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseSwaggerConfiguration();

app.UseAuthentication();

app.UseAuthorization();

app.UseApplicationExceptionHandling();

app.UseDomainExceptionHandling();

app.MapControllers();

app.Run();
