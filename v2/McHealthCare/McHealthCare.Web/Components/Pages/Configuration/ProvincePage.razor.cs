namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class ProvincePage
    {
        #region Variables

        private bool PanelVisible { get; set; } = true; 
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private List<CountryDto> Countries = [];
        private List<ProvinceDto> Provinces = [];

        private List<ExportFileData> ExportFileDatas =
        [
            new()
            {
                Column = "Country",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Code",
                Notes = "Mandatory"
            }
        ];

        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Variables

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            try
            {
                UserAccess = await UserService.GetUserInfo(ToastService);
                await LoadData(); 
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }
            }
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                Countries = await Mediator.Send(new GetCountryQuery());
                Provinces = await Mediator.Send(new GetProvinceQuery());
                SelectedDataItems = [];

                try
                {
                    Grid?.SelectRow(0, true);
                }
                catch { }

                PanelVisible = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteProvinceRequest(((ProvinceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<ProvinceDto>>();
                    await Mediator.Send(new DeleteProvinceRequest(Ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                var editModel = (ProvinceDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateProvinceRequest(editModel));
                else
                    await Mediator.Send(new UpdateProvinceRequest(editModel));

                await LoadData();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                PanelVisible = false;
            }
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Country", "Name", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var provinces = new List<ProvinceDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool IsValid = true;
                        var CountryId = Countries.FirstOrDefault(x => x.Name == ws.Cells[row, 1].Value?.ToString()?.Trim())?.Id ?? Guid.Empty;

                        if (CountryId == Guid.Empty)
                        {
                            ToastService.ShowErrorImport(row, 1, ws.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty);
                            IsValid = false;
                        }

                        if (!IsValid)
                            continue;

                        var Province = new ProvinceDto
                        {
                            CountryId = CountryId,
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim() ?? string.Empty,
                            Code = ws.Cells[row, 3].Value?.ToString()?.Trim() ?? string.Empty,
                        };

                        if (!Provinces.Any(x => x.CountryId == Province.CountryId && x.Code.Trim().ToLower() == Province?.Code?.Trim().ToLower() && x.Name.Trim().ToLower() == Province?.Name?.Trim().ToLower()))
                            provinces.Add(Province);
                    }

                    await Mediator.Send(new CreateListProvinceRequest(provinces));

                    await LoadData();

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }
    }
}