using TaSked.Application;
using TaSked.Infrastructure.Persistance;
using TaSked.Infrastructure.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddPersistance();
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateHomeworkCommand).Assembly));
builder.Services.AddJwtAuthentication(options =>
{
	options.Issuer = "https://localhost:5070/";
	options.Audience = "https://localhost:5070/";
	options.SecretKey = "TestSecretKey(HS256RequiersMin128BitLongKey)sdfgggggggggggggggggggggggggggggggggggggggsssssssssssssssssssssssssssssssssssfffffffffffffffffffffffffffffffffffffffffffffffffffffaaaaaaaa123456789009876543213456789098765432234567890987654336789098765434569AAAA";
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
