using Application.SendModels.AccountSendModels;
using Application.SendModels.Award;
using Application.SendModels.Category;
using Application.SendModels.Collection;
using Application.SendModels.Contest;
using Application.SendModels.EducationalLevel;
using Application.SendModels.Image;
using Application.SendModels.Notification;
using Application.SendModels.Painting;
using Application.SendModels.PaintingCollection;
using Application.SendModels.Post;
using Application.SendModels.Report;
using Application.SendModels.Resources;
using Application.SendModels.Round;
using Application.SendModels.RoundTopic;
using Application.SendModels.Schedule;
using Application.SendModels.Topic;
using FluentValidation;
using Infracstructures.SendModels.Painting;
using Infracstructures.SendModels.Sponsor;

namespace Application
{
    public interface IValidatorFactory
    {
        IValidator<TopicRequest> TopicRequestValidator { get; }
        IValidator<TopicUpdateRequest> TopicUpdateRequestValidator { get; }
        IValidator<AccountUpdateRequest> AccountUpdateRequestValidator { get; }
        IValidator<SubAccountRequest> SubAccountRequestValidator { get; }
        IValidator<AwardRequest> AwardRequestValidator { get; }
        IValidator<UpdateAwardRequest> UpdateAwardRequestValidator { get; }
        IValidator<CategoryRequest> CategoryRequestValidator { get; }
        IValidator<UpdateCategoryRequest> UpdateCategoryRequestValidator { get; }
        IValidator<CollectionRequest> CollectionRequestValidator { get; }
        IValidator<UpdateCollectionRequest> UpdateCollectionRequestValidator { get; }
        IValidator<ContestRequest> ContestRequestValidator { get; }
        IValidator<UpdateContest> UpdateContestRequestValidator { get; }
        IValidator<EducationalLevelRequest> EducationalLevelRequestValidator { get; }
        IValidator<EducationalLevelUpdateRequest> EducationalLevelUpdateRequestValidator { get; }
        IValidator<ImageRequest> ImageRequestValidator { get; }
        IValidator<NotificationRequest> NotificationRequestValidator { get; }
        IValidator<PaintingCollectionRequest> PaintingCollectionRequestValidator { get; }
        IValidator<CompetitorCreatePaintingRequest> CompetitorCreatePaintingRequestValidator { get; }
        IValidator<StaffCreatePaintingRequest> StaffCreatePaintingRequestValidator { get; }
        IValidator<PaintingUpdateStatusRequest> PaintingUpdateStatusRequestValidator { get; }
        IValidator<RatingRequest> RatingRequestValidator { get; }
        IValidator<UpdatePaintingRequest> UpdatePaintingRequestValidator { get; }
        IValidator<PostRequest> PostRequestValidator { get; }
        IValidator<PostUpdateRequest> UpdatePostRequestValidator { get; }
        IValidator<ReportRequest> ReportRequestValidator { get; }
        IValidator<UpdateReportRequest> UpdateReportRequestValidator { get; }
        IValidator<ResourcesUpdateRequest> ResourcesUpdateRequestValidator { get; }
        IValidator<ResourcesRequest> ResourcesRequestValidator { get; }
        IValidator<RoundTopicRequest> RoundTopicRequestValidator { get; }
        IValidator<RoundRequest> RoundRequestValidator { get; }
        IValidator<RoundUpdateRequest> RoundUpdateRequestValidator { get; }
        IValidator<ScheduleRequest> ScheduleRequestValidator { get; }
        IValidator<ScheduleUpdateRequest> ScheduleUpdateRequestValidator { get; }
        IValidator<ScheduleForFinalRequest> ScheduleForFinalRequestValidator { get; }
        IValidator<SponsorRequest> SponsorRequestValidator { get; }
        IValidator<SponsorUpdateRequest> SponsorUpdateRequestValidator { get; }
        IValidator<RoundTopicDeleteRequest> RoundTopicDeleteRequestValidator { get; }
        IValidator<FilterPaintingRequest> FilterPaintingRequestValidator { get; }
        IValidator<StaffCreatePaintingFinalRoundRequest> StaffCreatePaintingFinalRoundRequestValidator { get; }
        IValidator<StaffUpdatePaintingRequest> StaffUpdatePaintingRequestValidator { get; }
    }
}
