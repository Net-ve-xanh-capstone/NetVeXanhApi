using Application.IRepositories;

namespace Application;

public interface IUnitOfWork
{
    public IAccountRepository AccountRepo { get; }
    public IAwardRepository AwardRepo { get; }
    public IAwardScheduleRepository AwardScheduleRepo { get; }
    public ICollectionRepository CollectionRepo { get; }
    public IEducationalLevelRepository EducationalLevelRepo { get; }
    public IImageRepository ImageRepo { get; }
    public INotificationRepository NotificationRepo { get; }
    public IPaintingRepository PaintingRepo { get; }
    public IPaintingCollectionRepository PaintingCollectionRepo { get; }
    public IPostRepository PostRepo { get; }
    public IResourcesRepository ResourcesRepo { get; }
    public IRoundRepository RoundRepo { get; }
    public IScheduleRepository ScheduleRepo { get; }
    public ISponsorRepository SponsorRepo { get; }
    public ITopicRepository TopicRepo { get; }
    public IContestRepository ContestRepo { get; }
    public ICategoryRepository CategoryRepo { get; }
    public IReportRepository ReportRepo { get; }
    public IRoundTopicRepository RoundTopicRepo { get; }

    public Task<int> SaveChangesAsync();
}