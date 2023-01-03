using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity.Users
{
    public class User : IdentityUser<Guid>
    {
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set;}
    }
}