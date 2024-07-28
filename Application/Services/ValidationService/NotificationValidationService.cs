using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IService.IValidationService;

namespace Application.Services.ValidationService
{
    public class NotificationValidationService : INotificationValidationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Check Id is Exist
        public async Task<bool> IsExistedId(Guid id)
        {
            return await _unitOfWork.NotificationRepo.IsExistIdAsync(id);
        }
    }
}
