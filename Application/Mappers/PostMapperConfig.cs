using Application.SendModels.Post;
using Application.ViewModels.PostViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddPostMapperConfig()
    {
        CreateMap<Post, PostRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId))
            .ForMember(x => x.StaffId, x => x.MapFrom(x => x.CurrentUserId));
        CreateMap<Post, PostUpdateRequest>().ReverseMap()
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
        CreateMap<Post, PostResponse>().ReverseMap()
            .ForPath(x => x.Category.Id, x => x.MapFrom(x => x.CategoryId))
            .ForPath(x => x.Category.Name, x => x.MapFrom(x => x.CategoryName));

        CreateMap<Post, ListPostResponse>()
            .ForPath(x => x.Image, x => x.MapFrom(src => src.Images.Any() ? src.Images.First().Url : null))
            .ForPath(x => x.CategoryId, x => x.MapFrom(x => x.CategoryId))
            .ForPath(x => x.CategoryName, x => x.MapFrom(x => x.Category.Name));
    }
}