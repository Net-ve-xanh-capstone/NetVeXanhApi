using Application.SendModels.Contest;
using Application.SendModels.Round;
using Application.ViewModels.AccountViewModels;
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
        CreateMap<CreateRoundRequest, Round>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RoundStatus.NotStarted.ToString()))
            .ForPath(dest => dest.Award, opt => opt.MapFrom(src => src.Award));

        
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
        CreateMap<Round, RoundResponse>()
            .ForPath(dest => dest.EducationalLevelName, opt => opt.MapFrom(src => src.EducationalLevel.Level))
            .ForPath(dest => dest.ContestId, opt => opt.MapFrom(src => src.EducationalLevel.Contest.Id))
            .ForPath(dest => dest.ContestName, opt => opt.MapFrom(src => src.EducationalLevel.Contest.Name));
        CreateMap<Round, ListTopicResponse>().ReverseMap();

        CreateMap<Round, RoundInLevelViewModel>()
            .ForMember(dest => dest.Award, opt => opt.MapFrom(src => src.Award))
            .ForMember(dest => dest.RoundTopic, opt => opt.MapFrom(src => src.RoundTopic));


        CreateMap<Round, ListScheduleResponse>()
            .ForMember(des => des.RoundId, src => src.MapFrom(opt => opt.Id))
            .ForMember(des => des.RoundName, src => src.MapFrom(opt => opt.Name))
            .ForMember(des => des.EducationName, src => src.MapFrom(opt => opt.EducationalLevel.Level))
            .ForMember(des => des.Schedules,
                src => src.MapFrom(opt => opt.Schedule.Where(s => s.Status != ScheduleStatus.Delete.ToString())));

        CreateMap<Round, CompetitorResponse>()
            .ForPath(dest => dest.Id,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.Id)))
            .ForPath(dest => dest.Prize,
                opt => opt.MapFrom(src =>
                    src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Award!.Rank ?? "Không có giải thưởng")))
            .ForMember(dest => dest.RoundName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.Id)))
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.FullName)))
            .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src =>
                    src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => CalculateAge(p.Account.Birthday!.Value))))
            .ForMember(dest => dest.Birthday,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.Birthday)))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.Email)))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.Address)))
            .ForMember(dest => dest.Code,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.Code)))
            .ForMember(dest => dest.Phone,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.Phone)))
            .ForMember(dest => dest.Gender,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p =>
                    p.Account.Gender == true ? "Nữ" : p.Account.Gender == false ? "Nam" : null)))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.RoundTopic.SelectMany(rt => rt.Painting).Select(p => p.Account.Status)));

    }
}