using TaSked.Application;
using TaSked.Infrastructure.Persistance;
using TaSked.Infrastructure.Persistance.AzureMySqlInApp;
using TaSked.Infrastructure.Authorization;
using TaSked.Infrastructure.ExceptionHandling;
using TaSked.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);


bool useAzureMySqlInApp = builder.WebHost.GetSetting("UseAzureMySqlInApp")?.ToLower() == "true";
string JwtSecretKey = builder.WebHost.GetSetting("JwtSecretKey") ?? "{774F9515-F749-42F1-8578-8BA810C3BA78}";
string baseUrls = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey) ?? "http://localhost:5070";
var baseUrlList = baseUrls.Split(';');

builder.Services.AddControllers();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateHomeworkCommand).Assembly));
builder.Services.AddPolicyBasedAuthorization();
builder.Services.AddPersistance(useAzureMySqlInApp ? opt => opt.UseAzureMysqlInApp() : null);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddJwtAuthentication(options =>
{
	options.Issuer = baseUrlList.First();
	options.Audience = baseUrlList.First();
	options.SecretKey = JwtSecretKey;
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
