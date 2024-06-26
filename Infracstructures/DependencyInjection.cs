﻿using Application.IRepositories;
using Application.IService;
using Application.IService.ICommonService;
using Application.Mappers;
using Application.Services;
using Application.Services.CommonService;
using Infracstructures;
using Infracstructures.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInfractstructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

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

        #endregion

        #region Config validators

        //User Validator
        //services.AddTransient<IAccountValidator, AccountValidator>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        #endregion

        services.AddSingleton<IClaimsService, ClaimsService>();
        services.AddSingleton<ICurrentTime, CurrentTime>();
        services.AddSingleton<IMailService, MailService>();
        services.AddSingleton<ICacheServices, CacheServices>();

        // Use local DB
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("NetVeXanh")));


        services.AddAutoMapper(typeof(MapperConfigs).Assembly);


        return services;
    }
}