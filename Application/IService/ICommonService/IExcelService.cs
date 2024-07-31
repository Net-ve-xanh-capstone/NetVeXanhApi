namespace Application.IService.ICommonService;

public interface IExcelService
{
    Task<byte[]> GenerateExcel<T>(List<T> data, string name);
}