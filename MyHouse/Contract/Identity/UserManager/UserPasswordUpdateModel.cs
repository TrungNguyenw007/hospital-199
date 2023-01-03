using System.ComponentModel.DataAnnotations;

namespace Contract.Identity.UserManager
{
    public class UserPasswordUpdateModel
    {
        [MinLength(6)]
        public string UserName { get; set; }
        
        [MinLength(6)]
        public string? CurrentPassword { get; set; }
        
        [MinLength(6)]
        public string? NewPassword { get; set; }
        
        
        [Compare(nameof(NewPassword))]
        public string? NewPasswordConfirm { get; set; }
    }
}