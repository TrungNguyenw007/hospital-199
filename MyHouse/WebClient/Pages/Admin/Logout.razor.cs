using System.Threading.Tasks;

namespace WebClient.Pages.Admin
{
    public partial class Logout
    {
        
        protected override async Task OnInitializedAsync()
        {
            _navigationManager.NavigateTo("/login");
            _userManagerService.Logout();
        }

    }
}