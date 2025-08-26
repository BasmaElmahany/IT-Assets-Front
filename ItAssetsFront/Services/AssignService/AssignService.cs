using ItAssetsFront.Models.AssignDevice;
using ItAssetsFront.Models.BrandModels;
using ItAssetsFront.Models.DeviceModels;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace ItAssetsFront.Services.AssignService
{
    public class AssignService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:41335/api/EmployeeDeviceAssign";
        private readonly string _baseUrl2 = "http://localhost:41335/api/DeviceTransfer";
        public AssignService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all brands
        public async Task<List<GetAllAssigned>> GetAllDevicesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<GetAllAssigned>>($"{_baseUrl}/GetAll");
            return result ?? new List<GetAllAssigned>();
        }

     

        // Add new brand
        public async Task<Assign?> AddDeviceAsync(Assign deviceAss)
        {
            var payload = new
            {
                deviceID = deviceAss.deviceID,
                employeeID = deviceAss.employeeID,
                assignDate = deviceAss.assignDate.ToString("yyyy-MM-dd"), 
                deviceStatus = deviceAss.deviceStatus,
                qty = deviceAss.qty
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/EmpDeviceAssignment", deviceAss);
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Assign>>();
                return result?.data;
            }

            return null;
        }

        // Update brand
        public async Task<Return?> ReturnDeviceAsync(Return dev)
        {
            var payload = new
            {
                id = dev.id,
                deviceID = dev.deviceID,
                returnDate = dev.returnDate.ToString("yyyy-MM-dd"),
                returnStatus = dev.returnStatus,
                WhyReturn = dev.WhyReturn
            };
            await _httpClient.PutAsJsonAsync($"{_baseUrl}/ReturnDevice", payload);
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/ReturnDevice", dev);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Return>>();
                return result?.data;
            }

            return null;
        }
        public async Task<DeviceTransfer?> transferDeviceAsync(DeviceTransfer model)
        {
            var Payload = new
            {
                oldEmpId = model.oldEmpId,
                newEmpId = model.newEmpId,
                deviceID = model.deviceID,
                dateOnly = model.dateOnly.ToString("yyyy-MM-dd"),
            };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl2}/PostDeviceTransfer", Payload);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<DeviceTransfer>>();
                return result?.data;
            }

            return null;
        }
        // Delete brand
        public async Task<string?> DeleteAssignAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DeleteResponse>();
                return result?.message;
            }

            return null;
        }
     

        private class ApiResponse<T>
        {
            public T data { get; set; }
            public string message { get; set; }
        }

        private class DeleteResponse
        {
            public string message { get; set; }
        }

    }
}
