using ItAssetsFront.Models.BrandModels;
using ItAssetsFront.Models.DeviceRequest;

namespace ItAssetsFront.Services.DeviceRequestService
{
    public class RequestService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:41335/api/DeviceRequests";


        public RequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all brands
        public async Task<List<Request>> GetAllRequestsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Request>>($"{_baseUrl}/GetAll");
            return result ?? new List<Request>();
        }


        // Get brand by ID
        public async Task<Request?> GetReqByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Request>($"{_baseUrl}/GetbyId/{id}");
        }

        // Add new brand
        public async Task<PostRequest?> AddRequestAsync(PostRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/PostDeviceRequest", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PostRequest>>();
                return result?.data;
            }

            return null;
        }

        // Update brand
        public async Task<Request?> UpdateRequestAsync(EditRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/PutDeviceRequest/{request.id}", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Request>>();
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
