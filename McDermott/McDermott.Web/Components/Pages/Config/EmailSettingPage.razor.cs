using DevExpress.Data.XtraReports.Native;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class EmailSettingPage
    {
        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool PanelVisible { get; set; } = true;

        public IGrid Grid { get; set; }
        private List<CityDto> Cities = new();
        private List<ProvinceDto> Provinces = new();

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
    }
}