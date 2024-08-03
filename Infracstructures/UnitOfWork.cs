using Application;
using Application.IRepositories;

namespace Infracstructures;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;


    public UnitOfWork(AppDbContext context, 
        IAccountRepository accountRepository,
        IAwardRepository awardRepository,
        IAwardScheduleRepository awardScheduleRepository,
        ICollectionRepository collectionRepository,
        IEducationalLevelRepository educationalLevelRepository,
        IImageRepository imageRepository,
        INotificationRepository notificationRepository,
        IPaintingRepository paintingRepository,
        IPaintingCollectionRepository paintingCollectionRepository,
        IPostRepository postRepository,
        IResourcesRepository resourcesRepository,
        IRoundRepository roundRepository,
        IScheduleRepository scheduleRepository,
        ISponsorRepository sponsorRepository,
        ITopicRepository topicRepository,
        IContestRepository contestRepository,
        ICategoryRepository categoryRepository,
        IReportRepository reportRepository,
        IRoundTopicRepository roundTopicRepo,
        IDistrictRepository districtRepo)

    {
        _context = context;
        AccountRepo = accountRepository;
        AwardRepo = awardRepository;
        AwardScheduleRepo = awardScheduleRepository;
        CollectionRepo = collectionRepository;
        EducationalLevelRepo = educationalLevelRepository;
        ImageRepo = imageRepository;
        NotificationRepo = notificationRepository;
        PaintingRepo = paintingRepository;
        PaintingCollectionRepo = paintingCollectionRepository;
        PostRepo = postRepository;
        ResourcesRepo = resourcesRepository;
        RoundRepo = roundRepository;
        ScheduleRepo = scheduleRepository;
        SponsorRepo = sponsorRepository;
        TopicRepo = topicRepository;
        ContestRepo = contestRepository;
        CategoryRepo = categoryRepository;
        ReportRepo = reportRepository;
        RoundTopicRepo = roundTopicRepo;
        DistrictRepo = districtRepo;
    }

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
    
    public IDistrictRepository DistrictRepo { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}