namespace MVCview.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string role { get; set; }
        public int UserId { get; set; }
        public List<PermissionDto> permissions { get; set; }
    }
}
