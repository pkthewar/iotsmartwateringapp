using Microsoft.EntityFrameworkCore;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Hubs;
using SmartPlantWaterer.Services.Implementations;
using SmartPlantWaterer.Services.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(a => a.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddSingleton<IOnnxPredictionService, OnnxPredictionService>();
builder.Services.AddSingleton<IWateringRuleEngine, WateringRuleEngine>();
builder.Services.AddSingleton<IPumpService, PumpService>();
builder.Services.AddSingleton<ITelemetryService, TelemetryService>();
builder.Services.AddHostedService<MqttListenerService>();
builder.Services.AddSingleton<IHealthService, HealthService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<TelemetryHub>("/hubs/telemetry");

app.Run();
