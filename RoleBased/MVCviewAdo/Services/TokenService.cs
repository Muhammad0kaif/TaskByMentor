using MVCview.Models;
using System.Net.Http.Headers;

namespace MVCview.Services
{
    public class TokenService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> RefreshAccessToken()
        {
            var session = httpContextAccessor.HttpContext.Session;

            var refreshToken =
                session.GetString("refreshToken");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(
                    "https://localhost:7050/api/auth/refresh-token",
                    new
                    {
                        RefreshToken = refreshToken
                    });

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var result = await response.Content
                    .ReadFromJsonAsync<RefreshResponse>();

                session.SetString(
                    "token",
                    result.accessToken);
                session.SetString(
    "refreshToken",
    result.refreshToken);
                return true;
            }
        }
    }
}