using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IService.IValidationService;

namespace Application.Services.ValidationService
{
    public class RoundValidationService : IRoundValidationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoundValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Check Id is Exist
        public async Task<bool> IsExistedId(Guid id)
        {
            return await _unitOfWork.RoundRepo.IsExistIdAsync(id);
        }
    }
}
