using ItAssetsFront.Models.CategoryModels;


namespace ItAssetsFront.Services.CategoryService
{
    public class categoryService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:41335/api/Category";

        public categoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all brands
        public async Task<List<getAllCategories>> GetAllCategoryAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<getAllCategories>>($"{_baseUrl}/GetAll");
            return result ?? new List<getAllCategories>();
        }

        // Get brand by ID
        public async Task<getAllCategories?> GetCategoryByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<getAllCategories>($"{_baseUrl}/GetbyId/{id}");
        }

        // Add new brand
        public async Task<getAllCategories?> AddCategoryAsync(postCategory category)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/PostCategory", category);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<getAllCategories>>();
                return result?.data;
            }

            return null;
        }

        // Update brand
        public async Task<getAllCategories?> UpdateCategoryAsync(getAllCategories category)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/PutCategory/{category.id}", category);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<getAllCategories>>();
                return result?.data;
            }

            return null;
        }

        // Delete brand
        public async Task<string?> DeleteCategoryAsync(Guid id)
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
