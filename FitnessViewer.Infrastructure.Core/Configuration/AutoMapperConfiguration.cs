using AutoMapper;
using FitnessViewer.Infrastructure.Core.Helpers;
using FitnessViewer.Infrastructure.Core.Models;
using FitnessViewer.Infrastructure.Core.Models.Dto;
using FitnessViewer.Infrastructure.Core.Models.ViewModels;
using System;

namespace FitnessViewer.Infrastructure.Core.Configuration
{
    //public static class AutoMapperConfig
    //{
    //    public static void Configure()
    //    {
    //        Mapper.Initialize(cfg =>
    //        {
    //            cfg.AddProfile<InfrasturtureProfile>();
    //        });
    //    }

    public static class ObjectMapper
    {
        private static readonly System.Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<InfrasturtureProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }


    public class InfrasturtureProfile : Profile
        {
            public InfrasturtureProfile()
            {
                CreateMap<Strava.Athletes.Athlete, Athlete>()
                    .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => System.Convert.ToDecimal(src.Weight)));
                
                CreateMap<AthleteDto, Athlete>().ReverseMap();

                // endIndex is calculated from StartIndex by adding Duration. therefore we must copy Duration then StartIndex
                // so that it's calculated correctly.
                CreateMap<ActivityPeakDetailCalculator, ActivityPeakDetail>()
                    .ForMember(dest=>dest.Duration, opt=>opt.SetMappingOrder(1))
                    .ForMember(dest=>dest.StartIndex, opt=>opt.SetMappingOrder(2))
                    .ForMember(x => x.EndIndex, opt => opt.Ignore());
                
                CreateMap<Strava.Activities.ActivityLap, Lap>()
               //    .ForMember(src => src.Athlete, opt => opt.Ignore())
                   .ForMember(src => src.Activity, opt => opt.Ignore())
                   .ForMember(dest => dest.ElapsedTime, opts => opts.MapFrom(src => src.ElapsedTimeSpan))
                   .ForMember(dest => dest.MovingTime, opts => opts.MapFrom(src => src.MovingTimeSpan))
                 //  .ForMember(dest => dest.AthleteId, opts => opts.MapFrom(src => src.Athlete.Id))
                   .ForMember(dest => dest.ActivityId, opts => opts.MapFrom(src => src.Activity.Id));

                CreateMap<Notification, NotificationDto>().ReverseMap();

                CreateMap<ActivityDto, ActivityDetailDto>();

                CreateMap<AthleteDto, AthleteDashboardDto>();

                CreateMap<Activity, ActivityDetailDto>()
                            .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.StartDateLocal.Date.ToShortDateString()));

                CreateMap<Activity, EditActivityViewModel>();

            }
        }
    }
//}



    