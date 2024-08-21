using Application.SendModels.Resources;
using Application.ViewModels.ContestViewModels;
using Application.ViewModels.ResourcesViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddResourcesMapperConfig()
    {
        CreateMap<Resources, ResourcesRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));

        CreateMap<Resources, ResourcesUpdateRequest>().ReverseMap()
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
        CreateMap<Resources, ResourcesResponse>()
            .ForPath(dest => dest.SponsorName, opt => opt.MapFrom(src => src.Sponsor.Name))
            .ForPath(dest => dest.ContestName, opt => opt.MapFrom(src => src.Contest.Name));


        CreateMap<Resources, ResourceInContestViewModel>()
            .ForMember(x => x.Sponsor, x => x.MapFrom(x => x.Sponsor));
    }
}