namespace McDermott.Web.Extentions
{
    public interface ITestDataVillageService
    {
        IQueryable<Village> GetVillages();
    }
}