using System;
using Data.Models;

namespace Web.Pages.Photos
{
    public class IndividualPhotoModel
    {
        public string ImageUrl { get; set; }
        public DateTimeOffset TakenAt { get; set; }
        public string Description { get; set; }
    }
}