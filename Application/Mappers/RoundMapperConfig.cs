using Application.SendModels.Round;
using Application.ViewModels.ContestViewModels;
using Application.ViewModels.RoundViewModels;
using Application.ViewModels.ScheduleViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddRoundMapperConfig()
    {
        CreateMap<RoundRequest, Round>().ReverseMap()
            .ForMember(x => x.CurrentUserId, x => x.MapFrom(x => x.CreatedBy))
            .ForMember(x => x.CurrentUserId, x => x.MapFrom(x => x.UpdatedBy));
        CreateMap<RoundUpdateRequest, Round>().ReverseMap()
            .ForMember(x => x.CurrentUserId, x => x.MapFrom(x => x.UpdatedBy))
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null); // Kiểm tra srcMember không null
                opt.Condition((src, dest, srcMember, destMember) => // Kiểm tra nếu là Guid thì không Empty
                {
                    if (srcMember is Guid guidValue) return guidValue != Guid.Empty;
                    return true; // Cho phép ánh xạ nếu không phải kiểu Guid
                });
            });
        CreateMap<Round, RoundViewModel>()
            .ForPath(dest => dest.EducationalLevelName, opt => opt.MapFrom(src => src.EducationalLevel.Level))
            .ForPath(dest => dest.ContestId, opt => opt.MapFrom(src => src.EducationalLevel.Contest.Id))
            .ForPath(dest => dest.ContestName, opt => opt.MapFrom(src => src.EducationalLevel.Contest.Name));
        CreateMap<Round, ListTopicViewModel>().ReverseMap();

        CreateMap<Round, RoundInLevelViewModel>()
            .ForMember(dest => dest.RoundTopic, opt => opt.MapFrom(src => src.RoundTopic));


        CreateMap<Round, ListScheduleViewModel>()
            .ForMember(des => des.RoundId, src => src.MapFrom(opt => opt.Id))
            .ForMember(des => des.RoundName, src => src.MapFrom(opt => opt.Name))
            .ForMember(des => des.EducationName, src => src.MapFrom(opt => opt.EducationalLevel.Level))
            .ForMember(des => des.Schedules,
                src => src.MapFrom(opt => opt.Schedule.Where(s => s.Status != ScheduleStatus.Delete.ToString())));
    }
}