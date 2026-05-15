using System;
using System.Collections.Generic;
using System.Text;

namespace PocoClasses
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public ICollection<Order>? Orders { get; set; } 
    }
} 
