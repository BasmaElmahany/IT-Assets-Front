
using ItAssetsFront.Models.DeviceModels;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ItAssetsFront.Services.AssignService
{
    public class TransferService
    {
        private readonly string _baseUrl = "http://shusha.minya.gov.eg:85/api/DeviceTransfer";
        private readonly HttpClient _httpClient;

        public TransferService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DeviceTransfer>> GetAllDevicesTransferAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<DeviceTransfer>>($"{_baseUrl}/GetAll");
            return result ?? new List<DeviceTransfer>();
        }

        /// <summary>
        /// Export all transfers to Excel
        /// </summary>
        public async Task<byte[]> ExportTransfersToExcelAsync()
        {
            var transfers = await GetAllDevicesTransferAsync();
            if (transfers == null || !transfers.Any())
                return null;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Transfers");

                // Headers
                worksheet.Cells[1, 1].Value = "Device";
                worksheet.Cells[1, 2].Value = "Category";
                worksheet.Cells[1, 3].Value = "Brand";
                worksheet.Cells[1, 4].Value = "Old Employee";
                worksheet.Cells[1, 5].Value = "New Employee";
                worksheet.Cells[1, 6].Value = "Date";

                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                int row = 2;
                foreach (var t in transfers)
                {
                    worksheet.Cells[row, 1].Value = t.device?.name ?? "N/A";
                    worksheet.Cells[row, 2].Value = t.device?.category?.name ?? "N/A";
                    worksheet.Cells[row, 3].Value = t.device?.brand?.name ?? "N/A";
                    worksheet.Cells[row, 4].Value = t.emp1?.name ?? "N/A";
                    worksheet.Cells[row, 5].Value = t.emp2?.name ?? "N/A";
                    worksheet.Cells[row, 6].Value = t.dateOnly.ToString("yyyy-MM-dd");
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

    }
}
