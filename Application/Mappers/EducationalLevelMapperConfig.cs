﻿using Application.SendModels.EducationalLevel;
using Application.ViewModels.ContestViewModels;

using Application.ViewModels.ScheduleViewModels;

using Application.ViewModels.EducationalLevelViewModels;

using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddEducationalLevelMapperConfig()
    {
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
        CreateMap<EducationalLevel, EducationalLevelViewModel>()
            .ForMember(x => x.ContestId, x => x.MapFrom(x => x.ContestId));

        CreateMap<EducationalLevel, EducationalLevelInContest>()
            .ForMember(x => x.Award, x => x.MapFrom(x => x.Award))
            .ForMember(x => x.Round, x => x.MapFrom(x => x.Round));
    }
}