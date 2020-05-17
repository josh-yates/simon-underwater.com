using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Shared
{
    public class BasePageModel : PageModel
    {
        public string TabTitle { get; set; }
        public string PageTitle { get; set; }
        public string BackgroundImageUrl { get; set; }

        protected IActionResult RedirectToHome()
        {
            return LocalRedirect("~/");
        }

        protected IActionResult TryLocalRedirect(string redirectUrl)
        {
            if (redirectUrl == null || !Url.IsLocalUrl(redirectUrl))
            {
                return RedirectToHome();
            }

            return LocalRedirect(redirectUrl);
        }
    }
}