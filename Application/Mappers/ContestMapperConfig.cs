using Application.SendModels.Contest;
using Application.ViewModels.ContestViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddContestMapperConfig()
    {
        CreateMap<Contest, ContestViewModel>()
            .ForMember(dest => dest.AccountFullName, opt => opt.MapFrom(src => src.Account.FullName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status == ContestStatus.NotStarted.ToString() ? "Chưa bắt đầu" :
                src.Status == ContestStatus.InProcess.ToString() ? "Đang tiến hành" :
                src.Status == ContestStatus.Complete.ToString() ? "Đã Hoàn thành" :
                src.Status == ContestStatus.Delete.ToString() ? "Đã xóa" : null
            ));
        CreateMap<ContestViewModel, Contest>()
            .ForPath(dest => dest.Account.FullName, opt => opt.MapFrom(src => src.AccountFullName));

        CreateMap<Contest, ContestRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId))
            .ForMember(x => x.StaffId, x => x.MapFrom(x => x.CurrentUserId))
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null); // Kiểm tra srcMember không null
                opt.Condition((src, dest, srcMember, destMember) => // Kiểm tra nếu là Guid thì không Empty
                {
                    if (srcMember is Guid guidValue) return guidValue != Guid.Empty;
                    return true; // Cho phép ánh xạ nếu không phải kiểu Guid
                });
            });

        CreateMap<Contest, UpdateContest>().ReverseMap()
            .ForMember(x => x.UpdatedBy, x => x.MapFrom(x => x.CurrentUserId));

        CreateMap<Contest, ContestDetailViewModel>()
            .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account))
            .ForMember(dest => dest.Resource, opt => opt.MapFrom(src => src.Resources))
            .ForMember(dest => dest.EducationalLevel, opt => opt.MapFrom(src => src.EducationalLevel))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status == ContestStatus.NotStarted.ToString() ? "Chưa bắt đầu" :
                src.Status == ContestStatus.InProcess.ToString() ? "Đang tiến hành" :
                src.Status == ContestStatus.Complete.ToString() ? "Đã Hoàn thành" :
                src.Status == ContestStatus.Delete.ToString() ? "Đã xóa" : null
            )); 

        CreateMap<Contest, FilterPaintingContestViewModel>();


        CreateMap<Contest, ContestRewardViewModel>()
            .ForMember(dest => dest.AwardContestReward, opt => opt.MapFrom(src => src.EducationalLevel.SelectMany(level => level.Award)));

    }
}