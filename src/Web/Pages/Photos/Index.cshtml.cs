using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Data.Models;
using DataAccess.Utilities;
using Microsoft.EntityFrameworkCore;
using Web.Pages.Shared;
using Web.Services;

namespace Web.Pages.Photos
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _dbContext;
        public readonly ImageService _imageService;

        public IndexModel(
            AppDbContext dbContext,
            ImageService imageService
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }
        public PaginatedResult<Image> Images { get; set; }
        public async Task OnGetAsync()
        {
            Images = await _dbContext
                .Images
                .Where(i => !i.IsDeleted)
                .ToPaginatedResultAsync(1, 10);
        }
    }
}