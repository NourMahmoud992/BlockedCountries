using BlockedCountries.Helpers;
using BlockedCountries.Repositories;
using BlockedCountries.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<IpGeolocationSettings>(
	builder.Configuration.GetSection("IpGeolocation"));

builder.Services.AddHttpClient();
//builder.Services.AddSingleton<TemporaryBlockCleanupService>();
builder.Services.AddHostedService<TemporaryBlockCleanupService>();
builder.Services.AddSingleton<IBlockedCountriesRepository, BlockedCountriesRepository>();
builder.Services.AddSingleton<IBlockedAttemptsRepository, BlockedAttemptsRepository>();
builder.Services.AddScoped<IpGeolocationService>();
builder.Services.AddScoped<BlockedCountryService>();
builder.Services.AddScoped<BlockedAttempService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	// Load XML Comments
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
