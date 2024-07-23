using Application.BaseModels;
using Application.SendModels.Report;
using Application.ViewModels.ReportViewModels;
using FluentValidation;
using FluentValidation.Results;

namespace Application.IService;

public interface IReportService
{
    Task<bool> AddReport(ReportRequest addReportViewModel);
    Task<(List<ReportViewModel>, int)> GetAllReportPending(ListModels listAwardModel);
    Task<bool> DeleteReport(Guid reportId);
    Task<bool> UpdateReport(UpdateReportRequest updateReport);
    Task<ReportViewModel> GetReportById(Guid reportId);
    Task<(List<ReportViewModel>, int)> GetAllReport(ListModels listAwardModel);
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateReportRequest(ReportRequest report);
    Task<ValidationResult> ValidateReportUpdateRequest(UpdateReportRequest reportUpdate);
}