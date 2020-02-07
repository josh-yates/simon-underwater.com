using System;
using Microsoft.AspNetCore.Mvc;
using Web.Pages.Shared;

namespace Web.Pages.Photo
{
    public class EditModel : BasePageModel
    {
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public DateTime TakenAt { get; set; }

        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string TakenAtSourceMessage { get; set; }
        
        public void OnGet(int id)
        {
            Id = id;
        }
    }
}