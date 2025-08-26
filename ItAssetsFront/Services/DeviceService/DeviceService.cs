using ItAssetsFront.Models.AssignDevice;
using ItAssetsFront.Models.BrandModels;
using ItAssetsFront.Models.DeviceModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
        private readonly string _baseUrl = "http://localhost:41335/api/Device";
       
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
            formData.Add(new StringContent(model.Qty.ToString()), nameof(model.Qty));



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
            formData.Add(new StringContent(model.Qty.ToString()), nameof(model.Qty));

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

        internal async Task GetAllCategoryAsync()
        {
            throw new NotImplementedException();
        }




        public async Task<bool> CreateBulkDevicesAsync(BulkDeviceRequest model)
        {
            if (model.Devices == null || model.Devices.Count == 0)
                return false;

            bool allSuccess = true;

            foreach (var device in model.Devices)
            {
                // Assign shared properties
                device.BrandId = model.BrandId;
                device.CategoryID = model.CategoryID;
                device.SupplierID = model.SupplierID;

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



    }
}
