using Microsoft.EntityFrameworkCore;
using SmartPlantWaterer.Data;
using SmartPlantWaterer.Hubs;
using SmartPlantWaterer.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(a => a.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddSingleton<OnnxPredictionService>();
builder.Services.AddSingleton<WateringRuleEngine>();
builder.Services.AddSingleton<PumpService>();
builder.Services.AddSingleton<TelemetryService>();
builder.Services.AddHostedService<MqttListenerService>();

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
