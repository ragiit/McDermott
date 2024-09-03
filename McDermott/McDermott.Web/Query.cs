using McDermott.Persistence.Context;

namespace McDermott.Web
{
    public class Query
    {
        public IQueryable<Country> Countries([Service] ApplicationDbContext context) => context.Countries;

        public IQueryable<Village> Villages([Service] ApplicationDbContext context) => context.Villages;
    }
}