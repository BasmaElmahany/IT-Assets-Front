using ItAssetsFront.Models.BrandModels;

namespace ItAssetsFront.Services.BrandService
{
    public class brandService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://shusha.minya.gov.eg:85/api/Brand";

        public brandService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all brands
        public async Task<List<getAllBrands>> GetAllBrandsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<getAllBrands>>($"{_baseUrl}/GetAll");
            return result ?? new List<getAllBrands>();
        }

        // Get brand by ID
        public async Task<getAllBrands?> GetBrandByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<getAllBrands>($"{_baseUrl}/GetbyId/{id}");
        }

        // Add new brand
        public async Task<getAllBrands?> AddBrandAsync(postBrand brand)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/PostBrand", brand);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<getAllBrands>>();
                return result?.data;
            }

            return null;
        }

        // Update brand
        public async Task<getAllBrands?> UpdateBrandAsync(getAllBrands brand)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/PutBrand/{brand.id}", brand);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<getAllBrands>>();
                return result?.data;
            }

            return null;
        }

        // Delete brand
        public async Task<string?> DeleteBrandAsync(Guid id)
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
