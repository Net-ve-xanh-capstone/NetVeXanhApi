using Application.SendModels.PaintingCollection;
using Application.ViewModels.CollectionViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddPaintingCollectionMapperConfig()
    {
        CreateMap<PaintingCollection, PaintingCollectionRequest>().ReverseMap();

        CreateMap<PaintingCollection, PaintingCollectionInCollectionViewModel>()
            .ForMember(dest => dest.Painting, opt => opt.MapFrom(src => src.Painting));

        CreateMap<PaintingCollection, GetPaintingInCollectionPaintingCollectionViewModel>()
            .ForMember(dest => dest.Painting, opt => opt.MapFrom(src => src.Painting));
        
    }
}