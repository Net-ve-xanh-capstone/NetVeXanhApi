using Application.BaseModels;
using Application.ViewModels.SponsorViewModels;
using FluentValidation.Results;
using Infracstructures.SendModels.Sponsor;

namespace Application.IService;

public interface ISponsorService
{
    Task<bool> CreateSponsor(SponsorRequest sponsor);
    Task<(List<SponsorResponse>, int)> GetListSponsor(ListModels listModels);
    Task<SponsorResponse?> GetSponsorById(Guid id);
    Task<bool> UpdateSponsor(SponsorUpdateRequest updateSponsor);
    Task<bool> DeleteSponsor(Guid id);
    Task<List<SponsorResponse>> GetAllSponsor();
    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateSponsorRequest(SponsorRequest sponsor);
    Task<ValidationResult> ValidateSponsorUpdateRequest(SponsorUpdateRequest updateSponsor);
}