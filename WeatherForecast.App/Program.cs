using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WeatherForecast.App.Common;
using WeatherForecast.Infrastructure;
using WeatherForecast.Infrastructure.EntityFramework;
using WeatherForecast.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
{
    options
            .UseNpgsql(builder.Configuration.GetConnectionString("Weather"))
            .UseSnakeCaseNamingConvention();
    if (builder.Configuration.GetValue<bool>("LogSqlCommands"))
    {
        options.LogTo(Console.WriteLine);
    }
    
},
        contextLifetime: ServiceLifetime.Transient,
        optionsLifetime: ServiceLifetime.Singleton
);

builder.Services.AddHttpClient();
builder.Services.AddWeatherForecastServices();
builder.Services.AddHostedService<PrepareDbHostedService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {   {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[]
            {

            }
        }
    });
});

builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
