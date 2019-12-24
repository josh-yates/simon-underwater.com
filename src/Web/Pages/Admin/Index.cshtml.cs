using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Auth;

namespace Web.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public void OnGet() {
            DisplayName = User.FindFirst(Constants.FirstNameClaim).Value + " " + User.FindFirst(Constants.LastNameClaim).Value;
            EmailAddress = User.FindFirst(Constants.EmailClaim).Value;
        }
    }
}