using Application.IRepositories;
using Application.IService;
using Application.IService.ICommonService;
using Application.IValidators;
using Application.Jobs;
using Application.Mappers;
using Application.Services;
using Application.Services.CommonService;
using Infracstructures;
using Infracstructures.Repositories;
using Infracstructures.ScheduleTrigger;
using Infracstructures.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInfractstructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IValidatorFactory, ValidatorFactory>();

        #region Config Repository and Service

        //Authen
        services.AddTransient<IAuthentication, Authentication>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();

        // Account
        services.AddTransient<IAccountRepository, AccountRepository>();
        services.AddTransient<IAccountService, AccountService>();

        // Award
        services.AddTransient<IAwardRepository, AwardRepository>();
        services.AddTransient<IAwardService, AwardService>();

        // AwardSchedule
        services.AddTransient<IAwardScheduleRepository, AwardScheduleRepository>();
        services.AddTransient<IAwardScheduleService, AwardScheduleService>();

        // Collection
        services.AddTransient<ICollectionRepository, CollectionRepository>();
        services.AddTransient<ICollectionService, CollectionService>();

        // EducationalLevel
        services.AddTransient<IEducationalLevelRepository, EducationalLevelRepository>();
        services.AddTransient<IEducationalLevelService, EducationalLevelService>();


        // Image
        services.AddTransient<IImageRepository, ImageRepository>();
        services.AddTransient<IImageService, ImageService>();

        // Notification
        services.AddTransient<INotificationRepository, NotificationRepository>();
        services.AddTransient<INotificationService, NotificationService>();

        // Painting
        services.AddTransient<IPaintingRepository, PaintingRepository>();
        services.AddTransient<IPaintingService, PaintingService>();

        // PaintingCollection
        services.AddTransient<IPaintingCollectionRepository, PaintingCollectionRepository>();
        services.AddTransient<IPaintingCollectionService, PaintingCollectionService>();

        // Post
        services.AddTransient<IPostRepository, PostRepository>();
        services.AddTransient<IPostService, PostService>();


        // Resources
        services.AddTransient<IResourcesRepository, ResourcesRepository>();
        services.AddTransient<IResourcesService, ResourcesService>();

        // Round
        services.AddTransient<IRoundRepository, RoundRepository>();
        services.AddTransient<IRoundService, RoundService>();

        // Schedule
        services.AddTransient<IScheduleRepository, ScheduleRepository>();
        services.AddTransient<IScheduleService, ScheduleService>();

        // Sponsor
        services.AddTransient<ISponsorRepository, SponsorRepository>();
        services.AddTransient<ISponsorService, SponsorService>();

        // Topic
        services.AddTransient<ITopicRepository, TopicRepository>();
        services.AddTransient<ITopicService, TopicService>();

        // Contest
        services.AddTransient<IContestRepository, ContestRepository>();
        services.AddTransient<IContestService, ContestService>();

        //Category
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ICategoryService, CategoryService>();

        //Report
        services.AddTransient<IReportRepository, ReportRepository>();
        services.AddTransient<IReportService, ReportService>();

        //RoundTopic
        services.AddTransient<IRoundTopicRepository, RoundTopicRepository>();
        services.AddTransient<IRoundTopicService, RoundTopicService>();

        #endregion

        #region Config validators

        //Account Validator
        services.AddTransient<IAccountValidator, AccountValidator>();

        //AwardSchedule Validator
        services.AddTransient<IAwardScheduleValidator, AwardScheduleValidator>();

        //Award Validator
        services.AddTransient<IAwardValidator, AwardValidator>();

        //Category Validator
        services.AddTransient<ICategoryValidator, CategoryValidator>();

        //Collection Validator
        services.AddTransient<ICollectionValidator, CollectionValidator>();

        //Contest Validator
        services.AddTransient<IContestValidator, ContestValidator>();

        //EducationalLevel Validator
        services.AddTransient<IEducationalLevelValidator, EducationalLevelValidator>();

        //Image Validator
        services.AddTransient<IImageValidator, ImageValidator>();

        //Notification Validator
        services.AddTransient<INotificationValidator, NotificationValidator>();

        //PaintingCollection Validator
        services.AddTransient<IPaintingCollectionValidator, PaintingCollectionValidator>();

        //Painting Validator
        services.AddTransient<IPaintingValidator, PaintingValidator>();

        //Post Validator
        services.AddTransient<IPostValidator, PostValidator>();

        //Report Validator
        services.AddTransient<IReportValidator, ReportValidator>();

        //Resource Validator
        services.AddTransient<IResourceValidator, ResourceValidator>();

        //RoundTopic Validator
        services.AddTransient<IRoundTopicValidator, RoundTopicValidator>();

        //Round Validator
        services.AddTransient<IRoundValidator, RoundValidator>();

        //Schedule Validator
        services.AddTransient<IScheduleValidator, ScheduleValidator>();

        //Sponsor Validator
        services.AddTransient<ISponsorValidator, SponsorValidator>();

        //Topic Validator
        services.AddTransient<ITopicValidator, TopicValidator>();


        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        #endregion

        #region Config Quartz

        services.AddTransient<ISchedulerTrigger, SchedulerTrigger>();
        services.AddTransient<IJobFactory, JobFactory>();
        services.AddTransient<ISchedulerFactory, StdSchedulerFactory>();

        services.AddTransient<SchedulerTriggerJob>();
        var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        services.AddSingleton(new JobSchedule(
            jobType: typeof(SchedulerTriggerJob),
            cronExpression: "0 10 0 * * ?",
            timeZone: vietnamTimeZone
        ));

        #endregion

        services.AddSingleton<IClaimsService, ClaimsService>();
        services.AddSingleton<ICurrentTime, CurrentTime>();
        services.AddSingleton<IMailService, MailService>();
        services.AddSingleton<ICacheServices, CacheServices>();
        services.AddTransient<ISessionServices, SessionServices>();

        // Use local DB
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("NetVeXanh")));


        services.AddAutoMapper(typeof(MapperConfigs).Assembly);

        return services;
    }
}