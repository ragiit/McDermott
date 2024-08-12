using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using Microsoft.AspNetCore.SignalR.Client;

namespace McHealthCare.Web.Components.Pages.Medical
{
    public partial class ServicePage
    {
        #region Relation Data
        private List<ServiceDto> getService = [];
        private ServiceDto postService = new();
        #endregion
        #region Variabel
        private bool PanelVisible { get; set; } = false;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private HubConnection? hubConnection;

        private List<ExportFileData> ExportFileDatas =
        [
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            },
            
        ];

        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        #endregion

        #region Load data
        private async Task LoadData()
        {
            return;
        }
        #endregion
    }
}
