using ItAssetsFront.Models.EmployeeModels;
using ItAssetsFront.Models.SupplierModels;

namespace ItAssetsFront.Services.EmployeeService
{
    public class EmployeeService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://shusha.minya.gov.eg:85/api/Employee";

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<GetAllEmployee>> GetAllEmployeesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<GetAllEmployee>>($"{_baseUrl}/GetAll");
            return result ?? new List<GetAllEmployee>();
        }

   
        public async Task<GetAllEmployee?> GetEmployeeByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<GetAllEmployee>($"{_baseUrl}/GetbyId/{id}");
        }

      
        public async Task<GetAllEmployee?> AddEmployeeAsync(PostEmployee employee)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/PostEmployee", employee);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<GetAllEmployee>>();
                return result?.data;
            }

            return null;
        }

      
        public async Task<GetAllEmployee?> UpdateEmployeeAsync(GetAllEmployee employee)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/PutEmployee/{employee.id}", employee);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<GetAllEmployee>>();
                return result?.data;
            }

            return null;
        }

       
        public async Task<string?> DeleteEmployeeAsync(Guid id)
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

