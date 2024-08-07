using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infracstructures;
using Infracstructures.ScheduleTrigger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using WebAPI.Validation.AccountValidation;
using WebAPI.Validation.AwardValidation;
using WebAPI.Validation.CategoryValidation;
using WebAPI.Validation.CollectionValidation;
using WebAPI.Validation.ContestValidation;
using WebAPI.Validation.EducationalLevelValidation;
using WebAPI.Validation.ImageValidation;
using WebAPI.Validation.NotificationValidation;
using WebAPI.Validation.PaintingCollectionValidation;
using WebAPI.Validation.PaintingValidation;
using WebAPI.Validation.PostValidation;
using WebAPI.Validation.ReportValidation;
using WebAPI.Validation.ResourceValidation;
using WebAPI.Validation.RoundTopicValidation;
using WebAPI.Validation.RoundValidation;
using WebAPI.Validation.ScheduleValidation;
using WebAPI.Validation.SponsorValidation;
using WebAPI.Validation.TopicValidation;

namespace WebAPI;

public static class DependencyInjection
{
    public static void AddWebAPIService(this IServiceCollection services, WebApplicationBuilder builder,
        IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("NetVeXanh")));
        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        services.AddControllers().AddNewtonsoftJson(o =>
        {
            o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            // Specify the XML file path
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        services.AddHealthChecks();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        //services.AddScoped<IClaimsServices, ClaimsServices>();
        //Adding Session
        services.AddDistributedMemoryCache(); //Adding cache in memory for session.
        services.AddSession(); //Adding session.
        services.AddHostedService<QuartzHostedService>();
        var secretKeyBytes = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                //auto generate token
                ValidateIssuer = false,
                ValidateAudience = false,

                //sign in token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddFluentValidationClientsideAdapters();

        services.AddCors(options =>
        {
            options.AddPolicy("_myAllowSpecificOrigins",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });


        services.AddSwaggerGen(sw =>
        {
            sw.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "1.0" });
            sw.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insert JWT Token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            sw.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    public static IServiceCollection AddModelValidator(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<TopicRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<TopicUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<AccountUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SubAccountRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<AwardRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateAwardRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CategoryRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateCategoryRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CollectionRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateCollectionRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ContestRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateContestValidator>();
        services.AddValidatorsFromAssemblyContaining<EducationalLevelRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<EducationalLevelUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ImageRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<NotificationRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PaintingCollectionRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CompetitorCreatePaintingRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<StaffCreatePaintingRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PaintingUpdateStatusRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RatingRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePaintingRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PostRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatePostValidator>();
        services.AddValidatorsFromAssemblyContaining<ReportRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateReportRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ResourcesUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ResourcesRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RoundTopicRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RoundRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RoundUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ScheduleRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ScheduleUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ScheduleForFinalRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SponsorRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SponsorUpdateRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RoundTopicDeleteRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<FilterPaintingRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateAccountRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<StaffCreatePaintingForFinalRoundValidator>();
        services.AddValidatorsFromAssemblyContaining<StaffUpdatePaintingRequestValidator>();
        return services;
    }
}