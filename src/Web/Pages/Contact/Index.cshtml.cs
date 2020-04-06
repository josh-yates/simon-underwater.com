using System;
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
        public string UserEmail { get; set; }
        [BindProperty]
        // Honeypot field
        public string Email { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public void OnPostAsync()
        {
            
        }
    }
}