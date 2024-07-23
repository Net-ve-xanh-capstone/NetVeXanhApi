using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IService.ICommonService;
using Application.IService.IValidationService;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Application.Services.ValidationService
{
    public class AccountValidationService : IAccountValidationService 
    {
        private readonly IClaimsService _claimsService;
        private readonly IConfiguration _configuration;
        private readonly ICurrentTime _currentTime;

        // adding mapper in user service using DI
        private readonly IMapper _mapper;
        private readonly ISessionServices _sessionServices;
        private readonly IUnitOfWork _unitOfWork;

        public AccountValidationService(IUnitOfWork unitOfWork, ICurrentTime currentTime,
            IConfiguration configuration, ISessionServices sessionServices,
            IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _sessionServices = sessionServices;
            _claimsService = claimsService;
            _mapper = mapper;
        }
        //Check Id is Exist
        public async Task<bool> IsExistedId(Guid id)
        {
            return await _unitOfWork.AccountRepo.IsExistIdAsync(id);
        }
    }
}
