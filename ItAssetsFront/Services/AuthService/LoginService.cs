using ItAssetsFront.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ItAssetsFront.Services.AuthService
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        public LoginService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<string> LoginAsync(string _username, string _password)
        {
            var loginData = new
            {
                email = _username,
                password = _password
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://shusha.minya.gov.eg:85/api/Auth/login", content);

            string json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenObj = JsonConvert.DeserializeObject<TokenResponse>(json);
                Console.WriteLine(tokenObj.token);
                return tokenObj.token;
            }
            else
            {
                // Log the response body to see the error
                Console.WriteLine($"Status: {response.StatusCode}, Body: {json}");
                return null;
            }
        }

        public Dictionary<string, string> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = new Dictionary<string, string>();

            foreach (var claim in jwtToken.Claims)
            {
                claims[claim.Type] = claim.Value;
            }

            return claims;
        }
    }
}
