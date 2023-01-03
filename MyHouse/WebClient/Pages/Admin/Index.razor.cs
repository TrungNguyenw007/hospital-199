using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace WebClient.Pages.Admin
{
    public partial class Index : ComponentBase
    {
        private int number = 10;
        
        public Index()
        {
           
        }

        protected override async Task OnInitializedAsync()
        {
            
        }

        private async Task call()
        {
           await  _userManagerService.test();
        }
        
        
   
    }
}