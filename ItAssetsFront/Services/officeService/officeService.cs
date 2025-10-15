using ItAssetsFront.Models.CategoryModels;
using ItAssetsFront.Models.OfficeModels;

namespace ItAssetsFront.Services.officeService
{
    public class officeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://shusha.minya.gov.eg:85/api/Office";

        public officeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all brands
        public async Task<List<Office>> GetAllOfficeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Office>>($"{_baseUrl}/GetAll");
            return result ?? new List<Office>();
        }

        // Get brand by ID
        public async Task<Office?> GetOfficeByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Office>($"{_baseUrl}/GetbyId/{id}");
        }

        // Add new brand
        public async Task<Office?> AddOfficeAsync(postOffice Office)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/postOffice", Office);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Office>>();
                return result?.data;
            }

            return null;
        }

        // Update brand
        public async Task<Office?> UpdateOfficeAsync(Office office)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/PutOffice/{office.id}", office);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Office>>();
                return result?.data;
            }

            return null;
        }

        // Delete brand
        public async Task<string?> DeleteOfficeAsync(Guid id)
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
