using Data.Context;

namespace Web.Services.Data
{
    public interface IAppDbContextFactory
    {
        IAppDbContext Create();
    }
}