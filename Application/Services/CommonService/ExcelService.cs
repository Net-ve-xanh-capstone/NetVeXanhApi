using Application.IService.ICommonService;
using OfficeOpenXml;

namespace Application.Services.CommonService;

public class ExcelService : IExcelService
{
    public async Task<byte[]> GenerateExcel<T>(List<T> data, string name)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(name);

            if (data != null && data.Count > 0)
            {
                var properties = typeof(T).GetProperties();

                // Điền tiêu đề cột
                for (var i = 0; i < properties.Length; i++) worksheet.Cells[1, i + 1].Value = properties[i].Name;

                // Điền dữ liệu
                for (var i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    for (var j = 0; j < properties.Length; j++)
                        worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(item)?.ToString();
                }
            }

            return await Task.FromResult(package.GetAsByteArray());
        }
    }
}