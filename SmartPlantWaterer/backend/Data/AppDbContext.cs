using Microsoft.EntityFrameworkCore;
using SmartPlantWaterer.Models.DbModels;

namespace SmartPlantWaterer.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Telemetry> TelemetryLogs => Set<Telemetry>();

        public DbSet<HeartBeat> HeartBeatLogs => Set<HeartBeat>();

        public DbSet<Plants> Plants => Set<Plants>();
    }
}
