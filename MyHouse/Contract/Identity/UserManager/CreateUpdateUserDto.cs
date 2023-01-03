using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
// using Microsoft.AspNetCore.Http;

namespace Contract.Identity.UserManager
{
    public  class CreateUpdateUserDto
    {
        [MinLength(6)]
        public string UserName { get; set; }
        
        [MinLength(6)]
        public string? Password { get; set; }
        
        [Compare(nameof(Password))]
        public string? PasswordConfirm { get; set; }
        
        // [DataType(DataType.Upload)]
        // [FileExtensions(Extensions = "jpg,png")]
        // public IFormFile File { get; set;}
    }
}