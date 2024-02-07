namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class KioskPage
    {
        #region Relation Data

        public List<KioskDto> Kiosk = new();
        public List<ServiceDto> Service = new();
        public List<UserDto> Patients = new();
        public List<UserDto> Physician = new();

        #endregion Relation Data
    }
}