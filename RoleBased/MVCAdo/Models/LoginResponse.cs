namespace MVCAdo.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public int UserId { get; set; }
        public List<PermissionDto> permissions { get; set; }
    }
}
