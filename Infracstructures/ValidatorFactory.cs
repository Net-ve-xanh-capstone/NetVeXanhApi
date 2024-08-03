using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using IValidatorFactory = Application.IValidatorFactory;

namespace Infracstructures
{
    public class ValidatorFactory : IValidatorFactory
    {
        public IValidator<TopicRequest> TopicRequestValidator { get; }
        public IValidator<TopicUpdateRequest> TopicUpdateRequestValidator { get; }
        public IValidator<AccountUpdateRequest> AccountUpdateRequestValidator { get; }
        public IValidator<SubAccountRequest> SubAccountRequestValidator { get; }
        public IValidator<AwardRequest> AwardRequestValidator { get; }
        public IValidator<UpdateAwardRequest> UpdateAwardRequestValidator { get; }
        public IValidator<CategoryRequest> CategoryRequestValidator { get; }
        public IValidator<UpdateCategoryRequest> UpdateCategoryRequestValidator { get; }
        public IValidator<CollectionRequest> CollectionRequestValidator { get; }
        public IValidator<UpdateCollectionRequest> UpdateCollectionRequestValidator { get; }
        public IValidator<ContestRequest> ContestRequestValidator { get; }
        public IValidator<UpdateContest> UpdateContestRequestValidator { get; }
        public IValidator<EducationalLevelRequest> EducationalLevelRequestValidator { get; }
        public IValidator<EducationalLevelUpdateRequest> EducationalLevelUpdateRequestValidator { get; }
        public IValidator<ImageRequest> ImageRequestValidator { get; }
        public IValidator<NotificationRequest> NotificationRequestValidator { get; }
        public IValidator<PaintingCollectionRequest> PaintingCollectionRequestValidator { get; }
        public IValidator<CompetitorCreatePaintingRequest> CompetitorCreatePaintingRequestValidator { get; }
        public IValidator<StaffCreatePaintingRequest> StaffCreatePaintingRequestValidator { get; }
        public IValidator<PaintingUpdateStatusRequest> PaintingUpdateStatusRequestValidator { get; }
        public IValidator<RatingRequest> RatingRequestValidator { get; }
        public IValidator<UpdatePaintingRequest> UpdatePaintingRequestValidator { get; }
        public IValidator<PostRequest> PostRequestValidator { get; }
        public IValidator<PostUpdateRequest> UpdatePostRequestValidator { get; }
        public IValidator<ReportRequest> ReportRequestValidator { get; }
        public IValidator<UpdateReportRequest> UpdateReportRequestValidator { get; }
        public IValidator<ResourcesUpdateRequest> ResourcesUpdateRequestValidator { get; }
        public IValidator<ResourcesRequest> ResourcesRequestValidator { get; }
        public IValidator<RoundTopicRequest> RoundTopicRequestValidator { get; }
        public IValidator<RoundRequest> RoundRequestValidator { get; }
        public IValidator<RoundUpdateRequest> RoundUpdateRequestValidator { get; }
        public IValidator<ScheduleRequest> ScheduleRequestValidator { get; }
        public IValidator<ScheduleUpdateRequest> ScheduleUpdateRequestValidator { get; }
        public IValidator<ScheduleForFinalRequest> ScheduleForFinalRequestValidator { get; }
        public IValidator<SponsorRequest> SponsorRequestValidator { get; }
        public IValidator<SponsorUpdateRequest> SponsorUpdateRequestValidator { get; }
        public IValidator<RoundTopicDeleteRequest> RoundTopicDeleteRequestValidator { get; }
        public IValidator<FilterPaintingRequest> FilterPaintingRequestValidator { get; }

        public IValidator<StaffCreatePaintingFinalRoundRequest> StaffCreatePaintingFinalRoundRequestValidator { get; }

        public IValidator<StaffUpdatePaintingRequest> StaffUpdatePaintingRequestValidator { get; }

        public ValidatorFactory(
            IValidator<TopicRequest> topicRequestValidator,
            IValidator<TopicUpdateRequest> topicUpdateRequestValidator,
            IValidator<AccountUpdateRequest> accountUpdateRequestValidator,
            IValidator<SubAccountRequest> subAccountRequestValidator,
            IValidator<AwardRequest> awardRequestValidator,
            IValidator<UpdateAwardRequest> updateAwardRequestValidator,
            IValidator<CategoryRequest> categoryRequestValidator,
            IValidator<UpdateCategoryRequest> updateCategoryRequestValidator,
            IValidator<CollectionRequest> collectionRequestValidator,
            IValidator<UpdateCollectionRequest> updateCollectionRequestValidator,
            IValidator<ContestRequest> contestRequestValidator,
            IValidator<UpdateContest> updateContestRequestValidator,
            IValidator<EducationalLevelRequest> educationalLevelRequestValidator,
            IValidator<EducationalLevelUpdateRequest> educationalLevelUpdateRequestValidator,
            IValidator<ImageRequest> imageRequestValidator,
            IValidator<NotificationRequest> notificationRequestValidator,
            IValidator<PaintingCollectionRequest> paintingCollectionRequestValidator,
            IValidator<CompetitorCreatePaintingRequest> competitorCreatePaintingRequestValidator,
            IValidator<StaffCreatePaintingRequest> staffCreatePaintingRequestValidator,
            IValidator<PaintingUpdateStatusRequest> paintingUpdateStatusRequestValidator,
            IValidator<RatingRequest> ratingRequestValidator,
            IValidator<UpdatePaintingRequest> updatePaintingRequestValidator,
            IValidator<PostRequest> postRequestValidator,
            IValidator<PostUpdateRequest> updatePostRequestValidator,
            IValidator<ReportRequest> reportRequestValidator,
            IValidator<UpdateReportRequest> updateReportRequestValidator,
            IValidator<ResourcesUpdateRequest> resourcesUpdateRequestValidator,
            IValidator<ResourcesRequest> resourcesRequestValidator,
            IValidator<RoundTopicRequest> roundTopicRequestValidator,
            IValidator<RoundRequest> roundRequestValidator,
            IValidator<RoundUpdateRequest> roundUpdateRequestValidator,
            IValidator<ScheduleRequest> scheduleRequestValidator,
            IValidator<ScheduleUpdateRequest> scheduleUpdateRequestValidator,
            IValidator<ScheduleForFinalRequest> scheduleForFinalRequestValidator,
            IValidator<SponsorRequest> sponsorRequestValidator,
            IValidator<SponsorUpdateRequest> sponsorUpdateRequestValidator,
            IValidator<RoundTopicDeleteRequest> roundTopicDeleteRequestValidator,
            IValidator<FilterPaintingRequest> filterPaintingRequestValidator,
            IValidator<StaffCreatePaintingFinalRoundRequest> staffCreatePaintingFinalRoundRequestValidator,
            IValidator<StaffUpdatePaintingRequest> staffUpdatePaintingRequestValidator)
        {
            TopicRequestValidator = topicRequestValidator;
            TopicUpdateRequestValidator = topicUpdateRequestValidator;
            AccountUpdateRequestValidator = accountUpdateRequestValidator;
            SubAccountRequestValidator = subAccountRequestValidator;
            AwardRequestValidator = awardRequestValidator;
            UpdateAwardRequestValidator = updateAwardRequestValidator;
            CategoryRequestValidator = categoryRequestValidator;
            UpdateCategoryRequestValidator = updateCategoryRequestValidator;
            CollectionRequestValidator = collectionRequestValidator;
            UpdateCollectionRequestValidator = updateCollectionRequestValidator;
            ContestRequestValidator = contestRequestValidator;
            UpdateContestRequestValidator = updateContestRequestValidator;
            EducationalLevelRequestValidator = educationalLevelRequestValidator;
            EducationalLevelUpdateRequestValidator = educationalLevelUpdateRequestValidator;
            ImageRequestValidator = imageRequestValidator;
            NotificationRequestValidator = notificationRequestValidator;
            PaintingCollectionRequestValidator = paintingCollectionRequestValidator;
            CompetitorCreatePaintingRequestValidator = competitorCreatePaintingRequestValidator;
            StaffCreatePaintingRequestValidator = staffCreatePaintingRequestValidator;
            PaintingUpdateStatusRequestValidator = paintingUpdateStatusRequestValidator;
            RatingRequestValidator = ratingRequestValidator;
            UpdatePaintingRequestValidator = updatePaintingRequestValidator;
            PostRequestValidator = postRequestValidator;
            UpdatePostRequestValidator = updatePostRequestValidator;
            ReportRequestValidator = reportRequestValidator;
            UpdateReportRequestValidator = updateReportRequestValidator;
            ResourcesUpdateRequestValidator = resourcesUpdateRequestValidator;
            ResourcesRequestValidator = resourcesRequestValidator;
            RoundTopicRequestValidator = roundTopicRequestValidator;
            RoundRequestValidator = roundRequestValidator;
            RoundUpdateRequestValidator = roundUpdateRequestValidator;
            ScheduleRequestValidator = scheduleRequestValidator;
            ScheduleUpdateRequestValidator = scheduleUpdateRequestValidator;
            ScheduleForFinalRequestValidator = scheduleForFinalRequestValidator;
            SponsorRequestValidator = sponsorRequestValidator;
            SponsorUpdateRequestValidator = sponsorUpdateRequestValidator;
            RoundTopicDeleteRequestValidator = roundTopicDeleteRequestValidator;
            FilterPaintingRequestValidator = filterPaintingRequestValidator;
            StaffCreatePaintingFinalRoundRequestValidator = staffCreatePaintingFinalRoundRequestValidator;
            StaffUpdatePaintingRequestValidator = staffUpdatePaintingRequestValidator;
        }
    }

}
