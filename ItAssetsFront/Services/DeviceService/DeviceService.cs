using ItAssetsFront.Models.AssignDevice;
using ItAssetsFront.Models.BrandModels;
using ItAssetsFront.Models.DeviceModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace ItAssetsFront.Services.DeviceService
{
    public class DeviceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://shusha.minya.gov.eg:85/api/Device";
       
        public DeviceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/GetAllDevices");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Device>>(json);
        }

        public async Task<bool> CreateDeviceAsync(postDevice model)
        {
            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(model.Name), nameof(model.Name));
            formData.Add(new StringContent(model.SerialNumber), nameof(model.SerialNumber));
            formData.Add(new StringContent(model.Status), nameof(model.Status));
            formData.Add(new StringContent(model.Spex), nameof(model.Spex));
            formData.Add(new StringContent(model.Warranty.ToString()), nameof(model.Warranty));
            formData.Add(new StringContent(model.BrandId.ToString()), nameof(model.BrandId));
            formData.Add(new StringContent(model.CategoryID.ToString()), nameof(model.CategoryID));
            formData.Add(new StringContent(model.SupplierID.ToString()), nameof(model.SupplierID));
            formData.Add(new StringContent(model.IsFaulty.ToString()), nameof(model.IsFaulty));
            formData.Add(new StringContent(model.IsAvailable.ToString()), nameof(model.IsAvailable));
            formData.Add(new StringContent(model.price.ToString()), nameof(model.price));



            if (model.Photo != null)
            {
                var stream = model.Photo.OpenReadStream();
                formData.Add(new StreamContent(stream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue(model.Photo.ContentType) }
                }, nameof(model.Photo), model.Photo.FileName);
            }

            var response = await _httpClient.PostAsync($"{_baseUrl}/create-with-photo", formData);
            return response.IsSuccessStatusCode;
        }



        public async Task<bool> UpdateDeviceAsync(Guid id, EditDevice model)
        {
            // 💡 Add this block FIRST — fallback to existing photo URL if no new file uploaded
            if (model.Photo == null && !string.IsNullOrEmpty(model.ExistingPhotoUrl))
            {
                var photoFile = await DownloadFileAsFormFileAsync(
                    $"{_baseUrl.TrimEnd('/')}{model.ExistingPhotoUrl}",
                    Path.GetFileName(model.ExistingPhotoUrl)
                );
                model.Photo = photoFile;
            }

            using var formData = new MultipartFormDataContent();
            var deviceId = model.id;

            formData.Add(new StringContent(deviceId.ToString()), "id");
            formData.Add(new StringContent(model.Name), nameof(model.Name));
            formData.Add(new StringContent(model.SerialNumber), nameof(model.SerialNumber));
            formData.Add(new StringContent(model.Status), nameof(model.Status));
            formData.Add(new StringContent(model.Spex), nameof(model.Spex));
            formData.Add(new StringContent(model.Warranty.ToString()), nameof(model.Warranty));
            formData.Add(new StringContent(model.BrandId.ToString()), nameof(model.BrandId));
            formData.Add(new StringContent(model.CategoryID.ToString()), nameof(model.CategoryID));
            formData.Add(new StringContent(model.SupplierID.ToString()), nameof(model.SupplierID));
            formData.Add(new StringContent(model.IsFaulty.ToString()), nameof(model.IsFaulty));
            formData.Add(new StringContent(model.IsAvailable.ToString()), nameof(model.IsAvailable));
            formData.Add(new StringContent(model.price.ToString()), nameof(model.price));

            if (model.Photo != null)
            {
                var stream = model.Photo.OpenReadStream();
                formData.Add(new StreamContent(stream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue(model.Photo.ContentType) }
                }, nameof(model.Photo), model.Photo.FileName);
            }

            var response = await _httpClient.PutAsync($"{_baseUrl}/{id}/update-with-photo", formData);
            return response.IsSuccessStatusCode;
        }



        public async Task<bool> DeleteDeviceAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}/delete-with-photo");
            return response.IsSuccessStatusCode;
        }



        public async Task<bool> CreateBulkDevicesAsync(BulkDeviceRequest model)
        {
            if (model.Devices == null || model.Devices.Count == 0)
                return false;

            bool allSuccess = true;

            foreach (var d in model.Devices)
            {
                var device = new postDevice
                {
                    Name = model.Name,
                    SerialNumber = d.SerialNumber,
                    BrandId = model.BrandId,
                    CategoryID = model.CategoryID,
                    SupplierID = model.SupplierID,
                    Status = model.Status,
                    Spex = model.Spex,
                    Warranty = model.Warranty,
                    price = model.Price,
                    IsAvailable = model.IsAvailable,
                    IsFaulty = model.IsFaulty,
                    Photo = model.Photo
                };



                var success = await CreateDeviceAsync(device);
                if (!success)
                    allSuccess = false;
            }

            return allSuccess;
        }



        public async Task<IFormFile> DownloadFileAsFormFileAsync(string url, string fileName)
        {
            using var httpClient = new HttpClient();
            var fileBytes = await httpClient.GetByteArrayAsync(url);

            var stream = new MemoryStream(fileBytes);
            var formFile = new FormFile(stream, 0, stream.Length, "Photo", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream" 
            };

            return formFile;
        }


        /// <summary>
        /// Export all devices to Excel
        /// </summary>
        public async Task<byte[]> ExportDevicesToExcelAsync()
        {
            var devices = await GetAllDevicesAsync(); // Reuse your API call
            if (devices == null || !devices.Any())
                return null;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Devices");

                // Right-to-left layout
                worksheet.View.RightToLeft = true;

                // Headers
           
                worksheet.Cells[1, 2].Value = "اسم الجهاز";
                worksheet.Cells[1, 3].Value = "الرقم التسلسلي";
                worksheet.Cells[1, 4].Value = "الماركة";
                worksheet.Cells[1, 5].Value = "الفئة";
                worksheet.Cells[1, 6].Value = "المورد";
                worksheet.Cells[1, 7].Value = "الحالة";
                worksheet.Cells[1, 8].Value = "مواصفات";
                worksheet.Cells[1, 9].Value = "الضمان";
                worksheet.Cells[1, 10].Value = "السعر";
                worksheet.Cells[1, 11].Value = "متاح؟";
                worksheet.Cells[1, 12].Value = "به عطل؟";

                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 12])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Fill rows
                int row = 2;
                foreach (var d in devices)
                {
                    
                    worksheet.Cells[row, 2].Value = d.name;
                    worksheet.Cells[row, 3].Value = d.serialNumber;
                    worksheet.Cells[row, 4].Value = d.brand.name ?? "N/A";
                    worksheet.Cells[row, 5].Value = d.category.name ?? "N/A";
                    worksheet.Cells[row, 6].Value = d.supplier.name ?? "N/A";
                    worksheet.Cells[row, 7].Value = d.status;
                    worksheet.Cells[row, 8].Value = d.Spex;
                    worksheet.Cells[row, 9].Value = d.Warranty;
                    worksheet.Cells[row, 10].Value = d.price;
                    worksheet.Cells[row, 11].Value = d.isAvailable ? "نعم" : "لا";
                    worksheet.Cells[row, 12].Value = d.isFaulty ? "نعم" : "لا";
                    row++;
                }

                // Auto-fit
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                return package.GetAsByteArray();
            }
        }


    }
}
