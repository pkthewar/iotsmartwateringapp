using Microsoft.EntityFrameworkCore;
using SmartPlantWaterer.Models;

namespace SmartPlantWaterer.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Telemetry> TelemetryLogs => Set<Telemetry>();
    }
}
