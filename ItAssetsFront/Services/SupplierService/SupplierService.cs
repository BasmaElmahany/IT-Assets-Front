using ItAssetsFront.Models.OfficeModels;
using ItAssetsFront.Models.SupplierModels;

namespace ItAssetsFront.Services.SupplierService
{
    public class SupplierService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://shusha.minya.gov.eg:85/api/Supplier";

        public SupplierService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all brands
        public async Task<List<Supplier>> GetAllSupplierAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Supplier>>($"{_baseUrl}/GetAll");
            return result ?? new List<Supplier>();
        }

        // Get brand by ID
        public async Task<Supplier?> GetSupplierByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Supplier>($"{_baseUrl}/GetbyId/{id}");
        }

        // Add new brand
        public async Task<Supplier?> AddSupplierAsync(postSupplier supplier)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/PostSupplier", supplier);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Supplier>>();
                return result?.data;
            }

            return null;
        }

        // Update brand
        public async Task<Supplier?> UpdateSupplierAsync(Supplier supplier)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/PutSupplier/{supplier.id}", supplier);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Supplier>>();
                return result?.data;
            }

            return null;
        }

        // Delete brand
        public async Task<string?> DeleteSupplierAsync(Guid id)
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
