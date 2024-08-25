using Application.BaseModels;
using Application.IService;
using Application.IService.ICommonService;
using Application.SendModels.Notification;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using Infracstructures.ViewModels.NotificationViewModels;

namespace Application.Services;

public class NotificationService : INotificationService
{
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IMailService mailService)
    {
        _mailService = mailService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Create

    public async Task<bool> CreateNotification(NotificationRequest Notification)
    {
        var newNotification = _mapper.Map<Notification>(Notification);
        newNotification.Status = NotificationStatus.Active.ToString();
        newNotification.IsReaded = false;
        await _unitOfWork.NotificationRepo.AddAsync(newNotification);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion

    #region Get All

    public async Task<List<NotificationResponse>> Get5Notification(Guid id)
    {
        var list = await _unitOfWork.NotificationRepo.Get5NotificationOfUser(id);

        return _mapper.Map<List<NotificationResponse>>(list);
    }

    #endregion

    #region Is Read

    public async Task<bool> ReadNotification(Guid id)
    {
        var notification = await _unitOfWork.NotificationRepo.GetByIdAsync(id);
        if (notification == null) throw new Exception("Khong tim thay Notification");
        notification.IsReaded = true;

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    #endregion


    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.NotificationRepo.IsExistIdAsync(id);
    }

    #region Get By Id

    public async Task<NotificationDetailResponse?> GetNotificationById(Guid id)
    {
        var notification = await _unitOfWork.NotificationRepo.GetByIdAsync(id);
        if (notification == null) throw new Exception("Khong tim thay Notification");
        await ReadNotification(id);
        return _mapper.Map<NotificationDetailResponse>(notification);
    }

    public async Task<(List<NotificationResponse>?, int)> GetNotificationByAccountId(ListModels listModels, Guid id)
    {
        var accountList = await _unitOfWork.NotificationRepo.GetAllByAccount(id);
        accountList = accountList.Where(x => x.Status == AccountStatus.Inactive.ToString()).ToList();
        var result = _mapper.Map<List<NotificationResponse>>(accountList);

        var totalPages = (int)Math.Ceiling((double)result.Count / listModels.PageSize);
        int? itemsToSkip = (listModels.PageNumber - 1) * listModels.PageSize;
        result = result.Skip((int)itemsToSkip)
            .Take(listModels.PageSize)
            .ToList();
        return (result, totalPages);
    }

    public async Task<List<NotificationResponse>?> GetNotificationByAccountId(Guid id)
    {
        var notification = await _unitOfWork.NotificationRepo.GetByIdAsync(id);
        if (notification == null) throw new Exception("Khong tim thay Notification");
        await ReadNotification(id);
        return _mapper.Map<List<NotificationResponse>?>(notification);
    }

    #endregion
}