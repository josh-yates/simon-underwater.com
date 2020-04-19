using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Web.Pages.Shared;

namespace Web.Pages.Contact
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _dbContext;
        public IndexModel(
            AppDbContext dbContext
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        // Honeypot field
        public string AdditionalEmail { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Errors = GetErrors();

            if (!Errors.Any())
            {

            }

            return Page();
        }

        private List<string> GetErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Email))
            {
                errors.Add("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(Message))
            {
                errors.Add("Message is required.");
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add("Name is required");
            }

            return errors;
        }
    }
}