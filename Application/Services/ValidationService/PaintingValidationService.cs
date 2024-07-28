using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IService.IValidationService;

namespace Application.Services.ValidationService
{
    public class PaintingValidationService : IPaintingValidationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaintingValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Check Id is Exist
        public async Task<bool> IsExistedId(Guid id)
        {
            return await _unitOfWork.PaintingRepo.IsExistIdAsync(id);
        }
    }
}
