using Application.ViewModels.ContestViewModels;
using Application.ViewModels.SponsorViewModels;
using AutoMapper;
using Domain.Models;
using Infracstructures.SendModels.Sponsor;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddSponsorMapperConfig()
    {
        CreateMap<Sponsor, SponsorRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));

        CreateMap<Sponsor, SponsorUpdateRequest>().ReverseMap()
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

        CreateMap<Sponsor, SponsorResponse>().ReverseMap();

        CreateMap<Sponsor, SponsorInResourceViewModel>();
    }
}