using Application.SendModels.EducationalLevel;
using Application.ViewModels.ContestViewModels;
using Application.ViewModels.EducationalLevelViewModels;
using Application.ViewModels.ScheduleViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddEducationalLevelMapperConfig()
    {
        /*//Map For List Award
        CreateMap<EducationalLevel, ListAwardResponse>()
            .ForPath(des => des.AwardViewModels, opt => opt.MapFrom(src => src.Award));*/

        CreateMap<CreateEducationalLevelRequest, EducationalLevel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => EducationalLevelStatus.NotStarted.ToString()))
            .ForPath(dest => dest.Round, opt => opt.MapFrom(src => src.Round));


        CreateMap<EducationalLevel, EducationalLevelRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));
        CreateMap<EducationalLevel, EducationalLevelUpdateRequest>().ReverseMap()
            .ForMember(x => x.UpdatedBy, x => x.MapFrom(x => x.CurrentUserId))
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null); // Kiểm tra srcMember không null
                opt.Condition((src, dest, srcMember, destMember) => // Kiểm tra nếu là Guid thì không Empty
                {
                    if (srcMember is Guid guidValue) return guidValue != Guid.Empty;
                    return true; // Cho phép ánh xạ nếu không phải kiểu Guid
                });
            });
        CreateMap<EducationalLevel, EducationalLevelResponse>()
            .ForMember(x => x.ContestId, x => x.MapFrom(x => x.ContestId));

        CreateMap<EducationalLevel, EducationalLevelInContest>()
            .ForMember(x => x.Round, x => x.MapFrom(x => x.Round))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status == EducationalLevelStatus.NotStarted.ToString() ? "Chưa bắt đầu" :
                src.Status == EducationalLevelStatus.InProcess.ToString() ? "Đang tiến hành" :
                src.Status == EducationalLevelStatus.Complete.ToString() ? "Đã Hoàn thành" :
                src.Status == EducationalLevelStatus.Delete.ToString() ? "Đã xóa" : null
            ));
        ;

        CreateMap<EducationalLevel, ScheduleWebResponse>()
            .ForPath(x => x.ScheduleViewModels, x => x.MapFrom(x => x.Round.SelectMany(x => x.Schedule)));
    }
}