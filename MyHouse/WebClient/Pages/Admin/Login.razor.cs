using System.Threading.Tasks;
using Contract.Identity.UserManager;

namespace WebClient.Pages.Admin
{
    public partial class Login
    {
        
        private UserModel UserModel { get; set; } = new UserModel();


        public Login()
        {
            
        }

        private async Task SignIn()
        {
          await  _userManagerService.SignInAsync(UserModel);
          _navigationManager.NavigateTo("/",true);
            
        }

        private void RedirectToRegister()
        {
            _navigationManager.NavigateTo("register");
        }
    }
}