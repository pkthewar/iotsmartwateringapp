using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Hubs;
using SmartPlantWaterer.Services.Implementations;
using SmartPlantWaterer.Services.Interfaces;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", o =>
{
    o.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUPER_SECRET_KEY"))
    };
});

builder.Services.AddAuthorizationBuilder().AddPolicy("CanWater", p => p.RequireClaim("can_water", "true")).AddPolicy("CanOta", p => p.RequireClaim("can_ota", "true"));

builder.Services.AddControllers();

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(a => a.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IOnnxPredictionService, OnnxPredictionService>();
builder.Services.AddSingleton<IWateringRuleEngine, WateringRuleEngine>();
builder.Services.AddSingleton<IPumpService, PumpService>();
builder.Services.AddSingleton<ITelemetryService, TelemetryService>();
builder.Services.AddHostedService<MqttListenerService>();
builder.Services.AddSingleton<IHealthService, HealthService>();
builder.Services.AddScoped<IAlertChannel, TelegramAlertService>();
builder.Services.AddScoped<IAlertService, AlertService>();

WebApplication app = builder.Build();

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
app.MapHub<TelemetryHub>("/hubs/telemetry");

app.Run();
