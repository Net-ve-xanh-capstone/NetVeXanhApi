using Application.SendModels.AccountSendModels;
using Application.SendModels.Authentication;
using Application.SendModels.Painting;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.CollectionViewModels;
using Application.ViewModels.ContestViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddAccountMapperConfig()
    {
        CreateMap<CreateAccountRequest, Account>();
        CreateMap<StaffCreatePaintingRequest, Account>()
            //.ForMember(dest => dest.Id, src => src.MapFrom(opt => Guid.NewGuid()))
            .ForMember(dest => dest.Status, src => src.MapFrom(opt => AccountStatus.Active.ToString()))
            .ForMember(dest => dest.Role, src => src.MapFrom(opt => Role.Competitor.ToString()))
            .ForMember(dest => dest.Username, src => src.MapFrom(opt => Guid.NewGuid()));
        CreateMap<AccountUpdateRequest, Account>().ReverseMap();
        CreateMap<Account, AccountViewModel>().ReverseMap();

        CreateMap<Account, AccountAwardViewModel>();

        CreateMap<Account, AccountInPainting>();

        CreateMap<Account, AccountInContestViewModel>();
        CreateMap<Account, AccountValidationInfoViewModel>();

        CreateMap<Account, AccountRewardViewModel>()
            .ForMember(dest => dest.PaintingId, opt => opt.MapFrom(src => src.Painting.FirstOrDefault().Id))
            .ForMember(dest => dest.PaintingImage, opt => opt.MapFrom(src =>
                src.Painting
                    .Where(p => p.Status == PaintingStatus.HasPrizes.ToString())
                    .FirstOrDefault()
                    .Image
))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src =>
                src.Painting != null && src.Painting.Any()
                    ? GetRankInVietnamese(src.Painting.Where(p => p.Status == PaintingStatus.HasPrizes.ToString())
                    .FirstOrDefault().Award.Rank)
                    : "Không có giải"
                ))
            .ForPath(dest => dest.Gender, opt => opt.MapFrom(src =>
                src.Gender! == true ? "Nữ" :
                src.Gender! == false ? "Nam" : null)); ;

    }

    private int CalculateAge(DateTime birthday)
    {
        var today = DateTime.Today;
        var age = today.Year - birthday.Year;
        if (birthday.Date > today.AddYears(-age)) age--;
        return age;
    }
    private string GetRankInVietnamese(string rank)
    {
        return rank == RankAward.FirstPrize.ToString() ? "Giải Nhất" :
               rank == RankAward.SecondPrize.ToString() ? "Giải Nhì" :
               rank == RankAward.ThirdPrize.ToString() ? "Giải Ba" :
               rank == RankAward.ConsolationPrize.ToString() ? "Giải Tư" :
               rank == RankAward.Preliminary.ToString() ? "Qua Vòng Loại" : "Không có giải";
    }
}