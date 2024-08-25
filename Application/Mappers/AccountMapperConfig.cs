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
        CreateMap<CreateAccountV2Request, Account>();
        CreateMap<StaffCreatePaintingRequest, Account>()
            //.ForMember(dest => dest.Id, src => src.MapFrom(opt => Guid.NewGuid()))
            .ForMember(dest => dest.Status, src => src.MapFrom(opt => AccountStatus.Active.ToString()))
            .ForMember(dest => dest.Role, src => src.MapFrom(opt => Role.Competitor.ToString()))
            .ForMember(dest => dest.Username, src => src.MapFrom(opt => Guid.NewGuid()));
        CreateMap<AccountUpdateRequest, Account>().ReverseMap();
        CreateMap<Account, AccountResponse>().ReverseMap();

        CreateMap<Account, AccountAwardResponse>();

        CreateMap<Account, AccountInPainting>();

        CreateMap<Account, AccountInContestViewModel>();
        CreateMap<Account, AccountValidationInfoResponse>();

        CreateMap<Account, AccountRewardResponse>()
            .ForMember(dest => dest.PaintingId, opt => opt.MapFrom(src => src.Painting.FirstOrDefault().Id))
            .ForMember(dest => dest.PaintingImage, opt => opt.MapFrom(src =>
                src.Painting
                    .Where(p => p.Status == PaintingStatus.HasPrizes.ToString())
                    .FirstOrDefault()
                    .Image
            ))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => src.Painting
                .Where(p => p.Status == PaintingStatus.HasPrizes.ToString())
                .FirstOrDefault().Award.Rank
            ))
            .ForPath(dest => dest.Gender, opt => opt.MapFrom(src =>
                src.Gender! == true ? "Nữ" :
                src.Gender! == false ? "Nam" : null));
        ;

        CreateMap<Account, CompetitorResponse>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.Birthday!.Value)))
            .ForMember(dest => dest.Prize, opt => opt.Ignore())
            .ForMember(dest => dest.RoundName, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());
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