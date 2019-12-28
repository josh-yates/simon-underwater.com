using Web.Auth;
using Web.Pages.Shared;

namespace Web.Pages.Admin
{
    public class IndexModel : BasePageModel
    {
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public void OnGet() {
            DisplayName = User.FindFirst(Constants.FirstNameClaim).Value + " " + User.FindFirst(Constants.LastNameClaim).Value;
            EmailAddress = User.FindFirst(Constants.EmailClaim).Value;
        }
    }
}