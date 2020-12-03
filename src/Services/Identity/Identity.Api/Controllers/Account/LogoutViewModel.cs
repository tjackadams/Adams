namespace Adams.Services.Identity.Api.Controllers.Account
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}