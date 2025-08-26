using ItAssetsFront.Models.AssignDevice;
using ItAssetsFront.Models.AssignOffice;

namespace ItAssetsFront.Services.AssignService
{
    public class OfficeAssignService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:41335/api/OfficeDeviceAssign";
        public OfficeAssignService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all brands
        public async Task<List<getAllOfficeAssigned>> GetAllDevicesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<getAllOfficeAssigned>>($"{_baseUrl}/GetAll");
            return result ?? new List<getAllOfficeAssigned>();
        }



        // Add new brand
        public async Task<OfficeAssign?> AddDeviceAsync(OfficeAssign deviceAss)
        {
            var payload = new
            {
                deviceID = deviceAss.deviceID,
                OfficeID = deviceAss.OfficeID,
                assignDate = deviceAss.assignDate.ToString("yyyy-MM-dd"),
                deviceStatus = deviceAss.deviceStatus,
              //  qty = deviceAss.qty
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/OffDeviceAssignment", deviceAss);
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<OfficeAssign>>();
                return result?.data;
            }

            return null;
        }

        // Update brand
        public async Task<ReturndevOff?> ReturnDeviceAsync(ReturndevOff dev)
        {
            var payload = new
            {
                id = dev.id,
                deviceID = dev.deviceID,
                returnDate = dev.returnDate.ToString("yyyy-MM-dd"),
                returnStatus = dev.returnStatus
            };
            await _httpClient.PutAsJsonAsync($"{_baseUrl}/ReturnDevice", payload);
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/ReturnDevice", dev);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<ReturndevOff>>();
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
