using Application.Common;
using AutoMapper;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    public MapperConfigs()
    {
        //add map here
        //CreateMap<SourceModel, DestinationModel>();

        // Create mapping between Pagination
        CreateMap(typeof(Pagination<>), typeof(Pagination<>));

        AddAccountMapperConfig();

        AddAwardMapperConfig();

        AddAwardScheduleMapperConfig();

        AddCollectionMapperConfig();

        AddEducationalLevelMapperConfig();

        AddImageMapperConfig();

        AddNotificationMapperConfig();

        AddPaintingMapperConfig();

        AddPaintingCollectionMapperConfig();

        AddPostMapperConfig();

        AddResourcesMapperConfig();

        AddRoundMapperConfig();

        AddScheduleMapperConfig();

        AddSponsorMapperConfig();

        AddTopicMapperConfig();

        AddContestMapperConfig();

        AddCategoryMapperConfig();

        AddReportMapperConfig();

        AddRoundTopicMapperConfig();
    }

    partial void AddAccountMapperConfig();
    partial void AddAwardMapperConfig();
    partial void AddAwardScheduleMapperConfig();
    partial void AddCollectionMapperConfig();
    partial void AddEducationalLevelMapperConfig();
    partial void AddImageMapperConfig();
    partial void AddNotificationMapperConfig();
    partial void AddPaintingMapperConfig();
    partial void AddPaintingCollectionMapperConfig();
    partial void AddPostMapperConfig();
    partial void AddResourcesMapperConfig();
    partial void AddRoundMapperConfig();
    partial void AddScheduleMapperConfig();
    partial void AddSponsorMapperConfig();
    partial void AddTopicMapperConfig();
    partial void AddContestMapperConfig();
    partial void AddCategoryMapperConfig();
    partial void AddReportMapperConfig();
    partial void AddRoundTopicMapperConfig();
}