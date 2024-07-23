using Application.BaseModels;
using Application.ViewModels.SponsorViewModels;
using FluentValidation.Results;
using Infracstructures.SendModels.Sponsor;

namespace Application.IService;

public interface ISponsorService
{
    Task<bool> CreateSponsor(SponsorRequest sponsor);
    Task<(List<SponsorViewModel>, int)> GetListSponsor(ListModels listModels);
    Task<SponsorViewModel?> GetSponsorById(Guid id);
    Task<bool> UpdateSponsor(SponsorUpdateRequest updateSponsor);
    Task<bool> DeleteSponsor(Guid id);
    Task<List<SponsorViewModel>> GetAllSponsor();
    Task<bool> IsExistedId(Guid id);

    Task<ValidationResult> ValidateSponsorRequest(SponsorRequest sponsor);
    Task<ValidationResult> ValidateSponsorUpdateRequest(SponsorUpdateRequest updateSponsor);
}