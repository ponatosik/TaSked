using TaSked.Application;
using TaSked.Infrastructure.Persistance;
using TaSked.Infrastructure.Authorization;
using TaSked.Infrastructure.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddPersistance();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateHomeworkCommand).Assembly));
builder.Services.AddJwtAuthentication(options =>
{
	options.Issuer = "https://localhost:5070/";
	options.Audience = "https://localhost:5070/";
	options.SecretKey = "{774F9515-F749-42F1-8578-8BA810C3BA78}";
});
builder.Services.AddPolicyBasedAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseApplicationExceptionHandling();

app.UseDomainExceptionHandling();

app.MapControllers();

app.Run();
