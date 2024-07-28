using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IService.IValidationService;
using Application;

namespace Infracstructures
{
    public class ValidationServiceManager : IValidationServiceManager
    {
        public IAccountValidationService AccountValidationService { get; }
        public IAwardScheduleValidationService AwardScheduleValidationService { get; }
        public IAwardValidationService AwardValidationService { get; }
        public ICategoryValidationService CategoryValidationService { get; }
        public ICollectionValidationService CollectionValidationService { get; }
        public IContestValidationService ContestValidationService { get; }
        public IEducationalLevelValidationService EducationalLevelValidationService { get; }
        public IImageValidationService ImageValidationService { get; }
        public INotificationValidationService NotificationValidationService { get; }
        public IPaintingCollectionValidationService PaintingCollectionValidationService { get; }
        public IPaintingValidationService PaintingValidationService { get; }
        public IPostValidationService PostValidationService { get; }
        public IReportValidationService ReportValidationService { get; }
        public IResourceValidationService ResourceValidationService { get; }
        public IRoundTopicValidationService RoundTopicValidationService { get; }
        public IRoundValidationService RoundValidationService { get; }
        public IScheduleValidationService ScheduleValidationService { get; }
        public ISponsorValidationService SponsorValidationService { get; }
        public ITopicValidationService TopicValidationService { get; }

        public ValidationServiceManager(
            IAccountValidationService accountValidationService,
            IAwardScheduleValidationService awardScheduleValidationService,
            IAwardValidationService awardValidationService,
            ICategoryValidationService categoryValidationService,
            ICollectionValidationService collectionValidationService,
            IContestValidationService contestValidationService,
            IEducationalLevelValidationService educationalLevelValidationService,
            IImageValidationService imageValidationService,
            INotificationValidationService notificationValidationService,
            IPaintingCollectionValidationService paintingCollectionValidationService,
            IPaintingValidationService paintingValidationService,
            IPostValidationService postValidationService,
            IReportValidationService reportValidationService,
            IResourceValidationService resourceValidationService,
            IRoundTopicValidationService roundTopicValidationService,
            IRoundValidationService roundValidationService,
            IScheduleValidationService scheduleValidationService,
            ISponsorValidationService sponsorValidationService,
            ITopicValidationService topicValidationService)
        {
            AccountValidationService = accountValidationService;
            AwardScheduleValidationService = awardScheduleValidationService;
            AwardValidationService = awardValidationService;
            CategoryValidationService = categoryValidationService;
            CollectionValidationService = collectionValidationService;
            ContestValidationService = contestValidationService;
            EducationalLevelValidationService = educationalLevelValidationService;
            ImageValidationService = imageValidationService;
            NotificationValidationService = notificationValidationService;
            PaintingCollectionValidationService = paintingCollectionValidationService;
            PaintingValidationService = paintingValidationService;
            PostValidationService = postValidationService;
            ReportValidationService = reportValidationService;
            ResourceValidationService = resourceValidationService;
            RoundTopicValidationService = roundTopicValidationService;
            RoundValidationService = roundValidationService;
            ScheduleValidationService = scheduleValidationService;
            SponsorValidationService = sponsorValidationService;
            TopicValidationService = topicValidationService;
        }
    }
}
