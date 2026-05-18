using System;
using System.Collections.Generic;
using System.Text;

namespace PocoClasses.Dto
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int UserId { get; set; }
    }
}
