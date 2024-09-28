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
        CreateMap<CreateRoundRequest, Round>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => RoundStatus.NotStarted.ToString()))
            .ForPath(dest => dest.Award, opt => opt.MapFrom(src => src.Award))
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId))
            .ForMember(dest => dest.DeadlineSubmissionDate,
                       opt => opt.MapFrom(src => src.EndTime.AddDays(7))) // Tính 7 ngày sau từ EndTime
            .ForMember(dest => dest.ResultAnnouncementDate,
                       opt => opt.MapFrom(src => src.EndTime.AddDays(10))); // Tính 10 ngày sau từ EndTime


        CreateMap<RoundRequest, Round>().ReverseMap()
            .ForMember(x => x.CurrentUserId, x => x.MapFrom(x => x.CreatedBy))
            .ForMember(x => x.CurrentUserId, x => x.MapFrom(x => x.UpdatedBy))
            ;
        CreateMap<RoundUpdateRequest, Round>()
            .ForMember(x => x.UpdatedBy, x => x.MapFrom(x => x.CurrentUserId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom((src, dest) => src.Status ?? dest.Status))
            .ForMember(dest => dest.DeadlineSubmissionDate, opt => opt.MapFrom((src, dest) => src.DeadlineSubmissionDate ?? dest.DeadlineSubmissionDate))
            .ForMember(dest => dest.ResultAnnouncementDate, opt => opt.MapFrom((src, dest) => src.ResultAnnouncementDate ?? dest.ResultAnnouncementDate))
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
            .ForMember(dest => dest.RoundTopic, opt => opt.MapFrom(src => src.RoundTopic))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status == RoundStatus.NotStarted.ToString() ? "Chưa bắt đầu" :
                src.Status == RoundStatus.InProcess.ToString() ? "Đang tiến hành" :
                src.Status == RoundStatus.Complete.ToString() ? "Đã Hoàn thành" :
                src.Status == RoundStatus.Delete.ToString() ? "Đã xóa" : null
            ));
        ;
        ;


        CreateMap<Round, ListScheduleResponse>()
            .ForMember(des => des.RoundId, src => src.MapFrom(opt => opt.Id))
            .ForMember(des => des.RoundName, src => src.MapFrom(opt => opt.Name))
            .ForMember(des => des.RoundStatus, src => src.MapFrom(opt => opt.Status))
            .ForMember(des => des.EducationName, src => src.MapFrom(opt => opt.EducationalLevel.Level))
            .ForMember(des => des.Schedules,
                src => src.MapFrom(opt => opt.Schedule.Where(s => s.Status != ScheduleStatus.Delete.ToString())));


        // CreateMap<Round, CompetitorResponse>()
        //     .IncludeMembers(s => s.RoundTopic)
        //     .ForMember(dest => dest.RoundName, opt => opt.MapFrom(src => src.Name));
        //
    }
}