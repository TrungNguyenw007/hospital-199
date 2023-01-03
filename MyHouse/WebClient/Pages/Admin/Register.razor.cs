using System.Threading.Tasks;
using Contract.Identity.UserManager;

namespace WebClient.Pages.Admin
{
    public partial class Register
    {
        private CreateUpdateUserDto UserDto { get; set;} 
        private string Name { get; set; }
        
        public Register()
        {
            UserDto = new CreateUpdateUserDto();
        }

        protected override async Task OnInitializedAsync()
        {
            
        }

        public async Task HandleRegistration()
        {
            //call api
         var response = await _userManagerService.SignUpAsync(UserDto);
         
         
         
        }
        
   

    }
}