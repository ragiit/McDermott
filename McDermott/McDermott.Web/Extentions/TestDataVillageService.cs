using McDermott.Persistence.Context;

namespace McDermott.Web.Extentions
{
    public class TestDataVillageService(ApplicationDbContext _context) : ITestDataVillageService
    {
        public IQueryable<Village> GetVillages()
        {
            return _context.Villages.AsQueryable();
        }
    }
}