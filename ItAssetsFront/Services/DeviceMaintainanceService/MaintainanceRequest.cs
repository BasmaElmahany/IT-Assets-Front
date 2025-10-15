using ItAssetsFront.Models.DeviceMaintainance;
using ItAssetsFront.Models.DeviceRequest;

namespace ItAssetsFront.Services.DeviceMaintainanceService
{
    public class MaintainanceRequest
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://shusha.minya.gov.eg:85/api/DeviceMaintainanceSchedule";

        public MaintainanceRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DeviceMaintance>> GetAllSchedulesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<DeviceMaintance>>($"{_baseUrl}/GetAll");
            return result ?? new List<DeviceMaintance>();
        }
        public async Task<DeviceMaintance?> GetScheduleByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<DeviceMaintance>($"{_baseUrl}/GetbyId/{id}");
        }

        public async Task<PostDeviceMaintainance?> AddScheduleAsync(PostDeviceMaintainance request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/PostMaintainance", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PostDeviceMaintainance>>();
                return result?.data;
            }

            return null;
        }


        // Update brand
        public async Task<UpdateDeviceMaintainance?> UpdateRequestAsync(UpdateDeviceMaintainance request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/PutSchedule/{request.id}", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<UpdateDeviceMaintainance>>();
                return result?.data;
            }


            return null;
        }

        // Delete brand
        public async Task<string?> DeleteRequestAsync(Guid id)
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
