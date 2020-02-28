using Microsoft.AspNetCore.Mvc;
using Web.Pages.Shared;

namespace Web.Pages.Album
{
    public class EditModel : BasePageModel
    {
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }

        public int Id { get; set; }
    }
}