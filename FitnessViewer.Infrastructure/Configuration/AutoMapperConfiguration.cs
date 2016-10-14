using AutoMapper;
using FitnessViewer.Infrastructure.Models;

namespace FitnessViewer.Infrastructure.Configuration
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<InfrasturtureProfile>();
            });
        }

        public class InfrasturtureProfile : Profile
        {
            public InfrasturtureProfile()
            {
                CreateMap<Athlete, Strava.Athletes.Athlete>().ReverseMap();

                CreateMap<Strava.Activities.ActivityLap, Lap>()
                   .ForMember(src => src.Athlete, opt => opt.Ignore())
                   .ForMember(src => src.Activity, opt => opt.Ignore())
                   .ForMember(dest => dest.ElapsedTime, opts => opts.MapFrom(src => src.ElapsedTimeSpan))
                   .ForMember(dest => dest.MovingTime, opts => opts.MapFrom(src => src.MovingTimeSpan))
                   .ForMember(dest => dest.AthleteId, opts => opts.MapFrom(src => src.Athlete.Id))
                   .ForMember(dest => dest.ActivityId, opts => opts.MapFrom(src => src.Activity.Id));

                CreateMap<Notification, Models.Dto.NotificationDto>().ReverseMap();
            }
        }
    }
}




    