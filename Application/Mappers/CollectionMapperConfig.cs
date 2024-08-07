using Application.SendModels.Collection;
using Application.ViewModels.CollectionViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddCollectionMapperConfig()
    {
        CreateMap<Collection, CollectionRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId))
            .ForMember(x => x.AccountId, x => x.MapFrom(x => x.CurrentUserId));

        CreateMap<Collection, CollectionViewModel>()
            .ForMember(dest => dest.Image,
                opt => opt.MapFrom(src => src.PaintingCollection.Select(pc => pc.Painting.Image)));

        CreateMap<Collection, UpdateCollectionRequest>().ReverseMap()
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

        CreateMap<Collection, CollectionPaintingViewModel>()
            .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Account))
            .ForMember(dest => dest.PaintingCollection, opt => opt.MapFrom(src => src.PaintingCollection));

        CreateMap<Collection, GetPaintingInCollection>()
            .ForMember(dest => dest.Painting,
                opt => opt.MapFrom(src => src.PaintingCollection.Select(pc => pc.Painting)));
    }
}