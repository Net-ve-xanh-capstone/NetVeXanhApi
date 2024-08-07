using Application.SendModels.RoundTopic;
using Application.ViewModels.ContestViewModels;
using Application.ViewModels.TopicViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddRoundTopicMapperConfig()
    {
        CreateMap<RoundTopic, RoundTopicRequest>().ReverseMap();
        CreateMap<RoundTopic, RoundTopicViewModel>()
            .ForMember(des => des.Id, source => source.MapFrom(round => round.Id))
            .ForMember(des => des.Name, source => source.MapFrom(round => round.Topic.Name));

        CreateMap<RoundTopic, RoundTopicInContestViewModel>()
            .ForMember(x => x.Topic, x => x.MapFrom(x => x.Topic));

        CreateMap<RoundTopic, ListRoundTopicViewModel>()
            .ForMember(x => x.Name, x => x.MapFrom(x => x.Topic.Name));
    }
}