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

    public async Task<List<NotificationViewModel>> Get5Notification(Guid id)
    {
        var list = await _unitOfWork.NotificationRepo.Get5NotificationOfUser(id);

        return _mapper.Map<List<NotificationViewModel>>(list);
    }

    #endregion

    #region Get By Id

    public async Task<NotificationDetailViewModel?> GetNotificationById(Guid id)
    {
        var notification = await _unitOfWork.NotificationRepo.GetByIdAsync(id);
        if (notification == null) throw new Exception("Khong tim thay Notification");
        await ReadNotification(id);
        return _mapper.Map<NotificationDetailViewModel>(notification);
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


    #region Send result Round 2

    public async Task<bool> SendResultFinalRound(Guid id)
    {
        try
        {
            var paintings = await _unitOfWork.PaintingRepo.GetAllPaintingOfRound(id);
            var pass = paintings.Where(src => src.Status.Equals(PaintingStatus.HasPrizes.ToString())).ToList();
            foreach (var p in pass) await _mailService.PassPreliminaryRound(p.Account);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion

    #region Send result Round 1

    public async Task<bool> SendResultPreliminaryRound(Guid id)
    {
        try
        {
            var paintings = await _unitOfWork.PaintingRepo.GetAllPaintingOfRound(id);
            var pass = paintings.Where(src => src.Status.Equals(PaintingStatus.Pass.ToString())).ToList();
            foreach (var p in pass) await _mailService.PassPreliminaryRound(p.Account);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion

    //Check Id is Exist
    public async Task<bool> IsExistedId(Guid id)
    {
        return await _unitOfWork.NotificationRepo.IsExistIdAsync(id);
    }
}