using Application.SendModels.Schedule;
using Application.ViewModels.ScheduleViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddScheduleMapperConfig()
    {
        CreateMap<ScheduleRequest, Schedule>().ReverseMap()
            .ForMember(x => x.CurrentUserId, x => x.MapFrom(x => x.CreatedBy));
        CreateMap<ScheduleUpdateRequest, Schedule>().ReverseMap()
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
        CreateMap<Schedule, ScheduleViewModel>()
            .ForMember(x => x.Year, x => x.MapFrom(x => x.EndDate.Year.ToString()))
            .ForPath(x => x.Round, x => x.MapFrom(x => x.Round.Name))
            .ForPath(x => x.ExaminerName, x => x.MapFrom(x => x.Account.FullName));

        CreateMap<Schedule, ScheduleRatingViewModel>();
    }
}