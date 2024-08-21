using Application.BaseModels;
using Application.SendModels.Report;
using Application.ViewModels.ReportViewModels;
using FluentValidation.Results;

namespace Application.IService;

public interface IReportService
{
    Task<bool> AddReport(ReportRequest addReportViewModel);
    Task<(List<ReportResponse>, int)> GetAllReportPending(ListModels listAwardModel);
    Task<bool> DeleteReport(Guid reportId);
    Task<bool> UpdateReport(UpdateReportRequest updateReport);
    Task<ReportResponse> GetReportById(Guid reportId);
    Task<(List<ReportResponse>, int)> GetAllReport(ListModels listAwardModel);
    Task<bool> IsExistedId(Guid id);
    Task<ValidationResult> ValidateReportRequest(ReportRequest report);
    Task<ValidationResult> ValidateReportUpdateRequest(UpdateReportRequest reportUpdate);
}