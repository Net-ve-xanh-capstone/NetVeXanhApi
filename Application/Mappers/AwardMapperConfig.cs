using Application.SendModels.Award;
using Application.ViewModels.AwardViewModels;
using Application.ViewModels.ContestViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddAwardMapperConfig()
    {
        
        CreateMap<Award, AwardViewModel>().ReverseMap();

        CreateMap<CreateDependentAwardSendModel, Award>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AwardStatus.Active));

        CreateMap<Award, CreateAwardSendModel>().ReverseMap();
        CreateMap<Award, UpdateAwardRequest>().ReverseMap().ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null); // Kiểm tra srcMember không null
                opt.Condition((src, dest, srcMember, destMember) => // Kiểm tra nếu là Guid thì không Empty
                {
                    if (srcMember is Guid guidValue) return guidValue != Guid.Empty;
                    return true; // Cho phép ánh xạ nếu không phải kiểu Guid
                });
            });
        CreateMap<Award, AwardInLevelViewModel>();
    }
}