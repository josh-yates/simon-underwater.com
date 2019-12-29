using Web.Pages.Shared;

namespace Web.Pages.Photos
{
    public class IndexModel : BasePageModel
    {
        public bool IndividualPhoto { get; set; }
        public string Id { get; set; }
        public void OnGet(string id)
        {
            IndividualPhoto = !string.IsNullOrEmpty(id);

            if (IndividualPhoto)
            {
                Id = id;
            }
        }
    }
}