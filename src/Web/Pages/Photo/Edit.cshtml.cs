using Web.Pages.Shared;

namespace Web.Pages.Photo
{
    public class EditModel : BasePageModel
    {
        public int Id { get; set; }
        public void OnGet(int id)
        {
            Id = id;
        }
    }
}