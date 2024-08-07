using Application.IService.IValidationService;

namespace Application;

public interface IValidationServiceManager
{
    IAccountValidationService AccountValidationService { get; }
    IAwardScheduleValidationService AwardScheduleValidationService { get; }
    IAwardValidationService AwardValidationService { get; }
    ICategoryValidationService CategoryValidationService { get; }
    ICollectionValidationService CollectionValidationService { get; }
    IContestValidationService ContestValidationService { get; }
    IEducationalLevelValidationService EducationalLevelValidationService { get; }
    IImageValidationService ImageValidationService { get; }
    INotificationValidationService NotificationValidationService { get; }
    IPaintingCollectionValidationService PaintingCollectionValidationService { get; }
    IPaintingValidationService PaintingValidationService { get; }
    IPostValidationService PostValidationService { get; }
    IReportValidationService ReportValidationService { get; }
    IResourceValidationService ResourceValidationService { get; }
    IRoundTopicValidationService RoundTopicValidationService { get; }
    IRoundValidationService RoundValidationService { get; }
    IScheduleValidationService ScheduleValidationService { get; }
    ISponsorValidationService SponsorValidationService { get; }
    ITopicValidationService TopicValidationService { get; }
}