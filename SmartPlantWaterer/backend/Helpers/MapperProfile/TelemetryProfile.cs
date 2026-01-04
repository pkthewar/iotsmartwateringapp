using AutoMapper;
using SmartPlantWaterer.Models;
using SmartPlantWaterer.Models.DbModels;

namespace SmartPlantWaterer.Helpers.MapperProfile
{
    public class TelemetryProfile : Profile
    {
        public TelemetryProfile()
        {
            CreateMap<TelemetryPayload, Telemetry>();
        }
    }
}
