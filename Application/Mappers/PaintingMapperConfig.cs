﻿using Application.SendModels.Painting;
using Application.ViewModels.CollectionViewModels;
using Application.ViewModels.PaintingViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddPaintingMapperConfig()
    {
        CreateMap<Painting, CompetitorCreatePaintingRequest>().ReverseMap();

        CreateMap<Painting, StaffCreatePaintingRequest>().ReverseMap()
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));

        CreateMap<Painting, StaffCreatePaintingFinalRoundRequest>().ReverseMap()
            .ForMember(x => x.AccountId, x => x.MapFrom(x => x.CompetitorId))
            .ForMember(x => x.CreatedBy, x => x.MapFrom(x => x.CurrentUserId));


        CreateMap<Painting, PaintingViewModel>()
            .ForPath(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForPath(dest => dest.Birthday, opt => opt.MapFrom(src => src.Account.Birthday))
            .ForPath(dest => dest.Address, opt => opt.MapFrom(src => src.Account.Address))
            .ForPath(dest => dest.CompetitorCode, opt => opt.MapFrom(src => src.Account.Code))
            .ForPath(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForPath(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Account.FullName))
            .ForPath(dest => dest.TopicId, opt => opt.MapFrom(src => src.RoundTopic.Topic.Id))
            .ForPath(dest => dest.TopicName, opt => opt.MapFrom(src => src.RoundTopic.Topic.Name))
            .ForPath(dest => dest.RoundName, opt => opt.MapFrom(src => src.RoundTopic.Round.Name))
            .ForPath(dest => dest.Level, opt => opt.MapFrom(src => src.RoundTopic.Round.EducationalLevel.Level))
            .ForPath(dest => dest.ContestName,
                opt => opt.MapFrom(src => src.RoundTopic.Round.EducationalLevel.Contest.Name));

        CreateMap<PaintingViewModel, Painting>()
            .ForPath(dest => dest.Account.FullName, opt => opt.MapFrom(src => src.OwnerName))
            .ForPath(dest => dest.RoundTopic.Topic.Id, opt => opt.MapFrom(src => src.TopicId))
            .ForPath(dest => dest.RoundTopic.Topic.Name, opt => opt.MapFrom(src => src.TopicName))
            .ForPath(dest => dest.RoundTopic.Round.Name, opt => opt.MapFrom(src => src.RoundName))
            .ForPath(dest => dest.RoundTopic.Round.EducationalLevel.Level, opt => opt.MapFrom(src => src.Level))
            .ForPath(dest => dest.RoundTopic.Round.EducationalLevel.Contest.Name,
                opt => opt.MapFrom(src => src.ContestName));

        CreateMap<UpdatePaintingRequest, Painting>().ReverseMap()
            .ForMember(x => x.CurrentUserId, x => x.MapFrom(x => x.CreatedBy))
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null); // Kiểm tra srcMember không null
                opt.Condition((src, dest, srcMember, destMember) => // Kiểm tra nếu là Guid thì không Empty
                {
                    if (srcMember is Guid guidValue) return guidValue != Guid.Empty;
                    return true; // Cho phép ánh xạ nếu không phải kiểu Guid
                });
            });

        CreateMap<Painting, PaintingInCollectionViewModel>();

        CreateMap<Painting, PaintingInCollection2ViewModel>()
            .ForPath(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Account.FullName))
            .ForPath(dest => dest.TopicId, opt => opt.MapFrom(src => src.RoundTopic.Topic.Id))
            .ForPath(dest => dest.TopicName, opt => opt.MapFrom(src => src.RoundTopic.Topic.Name))
            .ForPath(dest => dest.ContestName,
                opt => opt.MapFrom(src => src.RoundTopic.Round.EducationalLevel.Contest.Name));
    }
}