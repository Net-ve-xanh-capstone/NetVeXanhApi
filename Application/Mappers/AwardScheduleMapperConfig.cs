using Application.ViewModels.ScheduleViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddAwardScheduleMapperConfig()
    {
        CreateMap<AwardSchedule, AwardScheduleResponse>()
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Award.Rank))
            .ForMember(dest => dest.PaintingViewModelsList, opt => opt.MapFrom(src => src.Schedule.Painting));
    }
}