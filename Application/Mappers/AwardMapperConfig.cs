using System.Linq;
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

        CreateMap<Award, AwardRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));

        CreateMap<Award, UpdateAwardRequest>().ReverseMap()
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
        CreateMap<Award, AwardInLevelViewModel>();

        CreateMap<Award, AwardContestRewardViewModel>()
            .ForMember(x => x.Level, x => x.MapFrom(x => x.EducationalLevel.Level))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src =>
                src.Rank == RankAward.FirstPrize.ToString() ? "Giải Nhất" :
                src.Rank == RankAward.SecondPrize.ToString() ? "Giải Nhì" :
                src.Rank == RankAward.ThirdPrize.ToString() ? "Giải Ba" :
                src.Rank == RankAward.ConsolationPrize.ToString() ? "Giải Tư" :
                src.Rank == RankAward.Preliminary.ToString() ? "Qua Vòng Loại" : "Không có giải"
            ))
            .ForMember(dest => dest.AccountReward, opt => opt.MapFrom(src => src.Painting.Select(p => p.Account).ToList()));
    }
}