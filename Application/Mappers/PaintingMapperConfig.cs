using Application.SendModels.Painting;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.CollectionViewModels;
using Application.ViewModels.PaintingViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddPaintingMapperConfig()
    {
        CreateMap<Painting, CompetitorCreatePaintingRequest>().ReverseMap();

        CreateMap<Painting, StaffCreatePaintingRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));

        CreateMap<Painting, StaffCreatePaintingFinalRoundRequest>().ReverseMap()
            .ForMember(x => x.AccountId, x => x.MapFrom(x => x.CompetitorId))
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));


        CreateMap<Painting, PaintingViewModel>()
            .ForPath(dest => dest.Award, opt => opt.MapFrom(src =>
                src.Award == null ? "Không có giải" :
                src.Award.Rank == RankAward.FirstPrize.ToString() ? "Giải Nhất" :
                src.Award.Rank == RankAward.SecondPrize.ToString() ? "Giải Nhì" :
                src.Award.Rank == RankAward.ThirdPrize.ToString() ? "Giải Ba" :
                src.Award.Rank == RankAward.ConsolationPrize.ToString() ? "Giải Tư" :
                src.Award.Rank == RankAward.Preliminary.ToString() ? "Qua Vòng Loại" : "Không có giải"
            ))
            .ForPath(dest => dest.Status, opt => opt.MapFrom(src =>
                src.Status == PaintingStatus.Draft.ToString() ? "Bản nháp" :
                src.Status == PaintingStatus.Submitted.ToString() ? "Đã nộp" :
                src.Status == PaintingStatus.Delete.ToString() ? "Đã xóa" :
                src.Status == PaintingStatus.Accepted.ToString() ? "Đã chấp nhận" :
                src.Status == PaintingStatus.Rejected.ToString() ? "Đã từ chối" :
                src.Status == PaintingStatus.Pass.ToString() ? "Qua Vòng 1" :
                src.Status == PaintingStatus.NotPass.ToString() ? "Không qua vòng 1" :
                src.Status == PaintingStatus.FinalRound.ToString() ? "Vòng chung kết" :
                src.Status == PaintingStatus.HasPrizes.ToString() ? "Có giải thưởng" :
                "Trạng thái không xác định"
            ))
            .ForPath(dest => dest.RoundId, opt => opt.MapFrom(src => src.RoundTopic.RoundId))
            .ForPath(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForPath(dest => dest.Birthday, opt => opt.MapFrom(src =>
                src.Account.Birthday.HasValue ? src.Account.Birthday.Value.ToString("dd/MM/yyyy") : string.Empty))
            .ForPath(dest => dest.Address, opt => opt.MapFrom(src => src.Account.Address))
            .ForPath(dest => dest.CompetitorCode, opt => opt.MapFrom(src => src.Account.Code))
            .ForPath(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForPath(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Account.FullName))
            .ForPath(dest => dest.OwnerRole,
                opt => opt.MapFrom(src => src.Account.Id == src.CreatedBy ? "Competitor" : "Staff"))
            .ForPath(dest => dest.TopicId, opt => opt.MapFrom(src => src.RoundTopic.Topic.Id))
            .ForPath(dest => dest.TopicName, opt => opt.MapFrom(src => src.RoundTopic.Topic.Name))
            .ForPath(dest => dest.RoundName, opt => opt.MapFrom(src => src.RoundTopic.Round.Name))
            .ForPath(dest => dest.Level, opt => opt.MapFrom(src => src.RoundTopic.Round.EducationalLevel.Level))
            .ForPath(dest => dest.ContestName,
                opt => opt.MapFrom(src => src.RoundTopic.Round.EducationalLevel.Contest.Name))
            .ForPath(dest => dest.ContestId,
                opt => opt.MapFrom(src => src.RoundTopic.Round.EducationalLevel.Contest.Id))
            .ForPath(dest => dest.RoundTopicId, opt => opt.MapFrom(src => src.RoundTopic.Id));

        CreateMap<Painting, PaintingTrackingViewModel>()
            .ForPath(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Account.FullName))
            .ForPath(dest => dest.History.Created.Time, opt =>
                opt.MapFrom(src => src.CreatedTime.HasValue ? src.CreatedTime : null))
            .ForPath(dest => dest.History.Created.Message, opt =>
                opt.MapFrom(src => src.CreatedTime.HasValue ? "Đã tạo" : null))
            .ForPath(dest => dest.History.Updated.Time, opt =>
                opt.MapFrom(src => src.UpdatedTime.HasValue ? src.UpdatedTime : null))
            .ForPath(dest => dest.History.Updated.Message, opt =>
                opt.MapFrom(src => src.UpdatedTime.HasValue ? "Đã sửa" : null))
            .ForPath(dest => dest.History.Reviewed.Time, opt =>
                opt.MapFrom(src => src.ReviewedTimestamp.HasValue ? src.ReviewedTimestamp : null))
            .ForPath(dest => dest.History.Reviewed.Message, opt =>
            {
                opt.MapFrom(src =>
                    src.ReviewedTimestamp.HasValue
                        ? src.Status == PaintingStatus.Rejected.ToString() ? "Không được duyệt" : "Đã được duyệt"
                        : null);
            })
            .ForPath(dest => dest.History.FinalDecision.Time, opt =>
                opt.MapFrom(src => src.FinalDecisionTimestamp.HasValue ? src.FinalDecisionTimestamp : null))
            .ForPath(dest => dest.History.FinalDecision.Message,
                opt => { opt.MapFrom(src => GetFinalDecisionMessage(src.FinalDecisionTimestamp, src.Status)); })
            .ForPath(dest => dest.History.Submitted.Time, opt =>
                opt.MapFrom(src => src.SubmittedTimestamp.HasValue ? src.SubmittedTimestamp : null))
            .ForPath(dest => dest.History.Submitted.Message, opt =>
                opt.MapFrom(src => src.SubmittedTimestamp.HasValue ? "Đã nộp bài" : null));


        CreateMap<PaintingViewModel, Painting>()
            .ForPath(dest => dest.Account.FullName, opt => opt.MapFrom(src => src.OwnerName))
            .ForPath(dest => dest.RoundTopic.Topic.Id, opt => opt.MapFrom(src => src.TopicId))
            .ForPath(dest => dest.RoundTopic.Topic.Name, opt => opt.MapFrom(src => src.TopicName))
            .ForPath(dest => dest.RoundTopic.Round.Name, opt => opt.MapFrom(src => src.RoundName))
            .ForPath(dest => dest.RoundTopic.Round.EducationalLevel.Level, opt => opt.MapFrom(src => src.Level))
            .ForPath(dest => dest.RoundTopic.Round.EducationalLevel.Contest.Name,
                opt => opt.MapFrom(src => src.ContestName));

        CreateMap<UpdatePaintingRequest, Painting>().ReverseMap()
            .ForMember(x => x.CurrentUserId, x => x.MapFrom(x => x.CreatedBy))
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null); // Kiểm tra srcMember không null
                opt.Condition((src, dest, srcMember, destMember) => // Kiểm tra nếu là Guid thì không Empty
                {
                    if (srcMember is Guid guidValue) return guidValue != Guid.Empty;
                    return true; // Cho phép ánh xạ nếu không phải kiểu Guid
                });
            });

        CreateMap<StaffUpdatePaintingRequest, Painting>()
            .ForPath(x => x.Account.Birthday, x => x.MapFrom(x => x.Birthday))
            .ForPath(x => x.Account.Phone, x => x.MapFrom(x => x.Phone))
            .ForPath(x => x.Account.FullName, x => x.MapFrom(x => x.FullName))
            .ForPath(x => x.Account.Address, x => x.MapFrom(x => x.Address))
            .ForPath(x => x.Account.Email, x => x.MapFrom(x => x.Email))
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));

        CreateMap<Painting, PaintingInCollectionViewModel>();

        CreateMap<Painting, PaintingInCollection2ViewModel>()
            .ForPath(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Account.FullName))
            .ForPath(dest => dest.OwnerRole, opt => opt.MapFrom(src => src.Account.Role))
            .ForPath(dest => dest.TopicId, opt => opt.MapFrom(src => src.RoundTopic.Topic.Id))
            .ForPath(dest => dest.TopicName, opt => opt.MapFrom(src => src.RoundTopic.Topic.Name))
            .ForPath(dest => dest.ContestName,
                opt => opt.MapFrom(src => src.RoundTopic.Round.EducationalLevel.Contest.Name))
            .ForMember(dest => dest.Rank, opt => opt.MapFrom(src =>
                src.Award == null ? "Không có giải" :
                src.Award.Rank == RankAward.FirstPrize.ToString() ? "Giải Nhất" :
                src.Award.Rank == RankAward.SecondPrize.ToString() ? "Giải Nhì" :
                src.Award.Rank == RankAward.ThirdPrize.ToString() ? "Giải Ba" :
                src.Award.Rank == RankAward.ConsolationPrize.ToString() ? "Giải Tư" :
                src.Award.Rank == RankAward.Preliminary.ToString() ? "Qua Vòng Loại" : "Không có giải"
            ));
        CreateMap<Painting, CompetitorViewModel>()
            .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.Account.Id))
            .ForPath(dest => dest.Prize, opt => opt.MapFrom(src => src.Award.Rank))
            .ForPath(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForPath(dest => dest.Code, opt => opt.MapFrom(src => src.Account.Code))
            .ForPath(dest => dest.Address, opt => opt.MapFrom(src => src.Account.Address))
            .ForPath(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForPath(dest => dest.FullName, opt => opt.MapFrom(src => src.Account.FullName))
            .ForPath(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.Account.Birthday!.Value)))
            .ForPath(dest => dest.Gender, opt => opt.MapFrom(src =>
                src.Account.Gender! == true ? "Nữ" :
                src.Account.Gender! == false ? "Nam" : null));
    }

    public static string GetFinalDecisionMessage(DateTime? finalDecisionTimestamp, string status)
    {
        if (!finalDecisionTimestamp.HasValue)
            return null;

        return status switch
        {
            // Sử dụng giá trị từ PaintingStatus
            var s when s == PaintingStatus.HasPrizes.ToString() => "Đã đoạt giải",
            var s when s == PaintingStatus.NotPass.ToString() => "Không qua vòng 1",
            var s when s == PaintingStatus.Pass.ToString() => "Đã qua vòng 1",
            _ => null
        };
    }
}