namespace WebClient.Shared
{
    public partial class LoginRedirect
    {
        protected override void OnAfterRender(bool firstRender)
        {
            _navigationManager.NavigateTo("login");
        }

       
    }
}