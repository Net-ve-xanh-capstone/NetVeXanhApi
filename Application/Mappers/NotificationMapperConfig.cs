using Application.SendModels.Notification;
using AutoMapper;
using Domain.Models;
using Infracstructures.ViewModels.NotificationViewModels;

namespace Application.Mappers;

public partial class MapperConfigs : Profile
{
    partial void AddNotificationMapperConfig()
    {
        CreateMap<NotificationRequest, Notification>();
        CreateMap<Notification, NotificationResponse>().ReverseMap();
        CreateMap<Notification, NotificationDetailResponse>().ReverseMap();
    }
}