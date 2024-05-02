namespace McDermott.Web.Components.Pages.Patient
{
    public partial class InsurancePolicyPage
    {
        [Parameter]
        public bool IsPopUpForm { get; set; } = false;

        private List<CountryDto> Countries = [];
        private List<ProvinceDto> Provinces = [];

        [Parameter]
        public UserDto User { get; set; } = new()
        {
            Name = "-"
        };

        private List<UserDto> Users = [];
        private List<InsuranceDto> Insurances = [];
        private List<InsurancePolicyDto> InsurancePolicies = [];
        private InsurancePolicyDto InsurancePoliciyForm = new();

        #region Data

        private bool IsBPJS = false;
        private long? _InsuranceId = 0;

        private long? InsuranceId
        {
            get => _InsuranceId;
            set
            {
                InsurancePoliciyForm.InsuranceId = (long)value;
                _InsuranceId = value;

                IsBPJS = Insurances.Any(x => x.IsBPJS == true && x.Id == value) ? true : false;
            }
        }

        #endregion Data

        #region Grid Properties

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    await GetUserInfo();
                }
                catch { }
            }
        }

        private async Task GetUserInfo()
        {
            try
            {
                var user = await UserInfoService.GetUserInfo();
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        private bool ShowForm { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        private bool FormValidationState = true;

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        #endregion Grid Properties

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            Insurances = await Mediator.Send(new GetInsuranceQuery());

            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            ShowForm = false;
            SelectedDataItems = new ObservableRangeCollection<object>();
            Countries = await Mediator.Send(new GetCountryQuery());

            InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery());

            if (User != null && User.Id != 0)
            {
                InsurancePolicies = InsurancePolicies.Where(x => x.UserId == User.Id).ToList();
            }

            PanelVisible = false;
        }

        #endregion LoadData

        #region Grid Function

        private void SelectedItemInsuranceChanged(InsuranceDto e)
        {
            if (e is null)
                return;

            IsBPJS = Insurances.Any(x => x.IsBPJS == true && x.Id == e.Id);
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #region SaveDelete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteInsurancePolicyRequest(((InsuranceDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteInsurancePolicyRequest(ids: SelectedDataItems.Adapt<List<InsuranceDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch { }
        }

        private async Task OnSave()
        {
            try
            {
                ToastService.ClearInfoToasts();
                if (!FormValidationState)
                {
                    ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(User.Name))
                {
                    ToastService.ShowInfo("Please select the Patient first.");
                    return;
                }

                InsurancePoliciyForm.UserId = User.Id;

                if (InsurancePoliciyForm.Id == 0)
                    await Mediator.Send(new CreateInsurancePolicyRequest(InsurancePoliciyForm));
                else
                    await Mediator.Send(new UpdateInsurancePolicyRequest(InsurancePoliciyForm));

                await LoadData();
            }
            catch { }
        }

        #endregion SaveDelete

        #region ToolBar Button

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Name", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString().Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match the grid.");
                        return;
                    }

                    var countries = new List<CountryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new CountryDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim()
                        };

                        if (!Countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()) && !countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()))
                            countries.Add(country);
                    }

                    await Mediator.Send(new CreateListCountryRequest(countries));

                    await LoadData();
                }
                catch { }
            }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void NewItem_Click()
        {
            InsurancePoliciyForm = new();
            ShowForm = true;
        }

        private void EditItem_Click()
        {
            try
            {
                InsurancePoliciyForm = SelectedDataItems[0].Adapt<InsurancePolicyDto>();
                ShowForm = true;
            }
            catch { }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile");
        }

        #endregion ToolBar Button

        #endregion Grid Function

        #region Form

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            await OnSave();
        }

        private bool IsLoadingGetBPJS { get; set; } = false;
        private BPJSIntegrationDto BPJSIntegration { get; set; } = new();

        private async Task OnClickGetBPJS()
        {
            if (string.IsNullOrWhiteSpace(InsurancePoliciyForm.PolicyNumber))
            {
                ToastService.ShowInfo("Please insert the Policy Number!");
                return;
            }

            IsLoadingGetBPJS = true;

            var result = await PcareService.SendPCareService($"peserta/{InsurancePoliciyForm.PolicyNumber}", HttpMethod.Get);
            if (result.Item2 == 200)
            {
                BPJSIntegration = System.Text.Json.JsonSerializer.Deserialize<BPJSIntegrationDto>(result.Item1);
            }
            else
            {
                BPJSIntegration = new();
                dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                string message = data.metaData.message;
                int code = data.metaData.code;

                ToastService.ShowError($"{message}\n Code: {code}");
            }

            IsLoadingGetBPJS = false;

            //DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //TimeSpan timeSpan = DateTime.UtcNow - epoch;
            //long tStamp = (long)timeSpan.TotalSeconds;

            //string r = "Wcem5CYDCbnmR5fcoBtip3UDpHUxcq1N7hUfK4uMm3xs3L+vPboELhK6PMjvxX1QP0o9DpVcEsLBjmswszbku3pFH2HR3S+iLcSP65JFYJlGwXUDtd9TF+PFQuPo3YaqRGhLQ3YBLCkPifnTcuGqAQdIw5cNT5RhF8tKcmOWB6tTc29heTSp5mTf4S81FDl9jY55n1OkQsirSzYGgGzXdolW4K6mMc6loYstTR1GVQXaTEsl7z4HhzXtEVB7W2IYaTcrUetUW6Tad0EGaaAznwOs8ek8YDd+4CQcbFCq/K30RZg9uiKyLeFtfsQCyA9ZDOS7sKhTUdGiGqIyW6uLsBSEq4ysZJpMcIZc/8D0aps56auvLmJY+HNWKklfbl88+DJmpxud7hFuYZo9jTlkrIA/gnIJZcKTa6gMUpzEH1RyHdzOTNogRxvpwGb3dRkcHfjvOHo0kI9DM2236XWfjIypa2RkhF8KsKtBjoSnVnMNDZNTXX6Sp/k/vmaJcU+8RJbh1ah1E1tgLnyGQY7+nu8CjCBqv0ODOChG7JKKz6dU6KZC/VWX7cXH8+/SGXOSKiijsJJEAiOX2Okqv87m9rmhHL6VcoCNFhQXtOYo1U2RCQVXhcpxOsl6gsQWqAMYFc95zMSjkBCot6975LQVhETzt2AAd67L6Abd+ylRBzlpfX2PZrIab0i8rjwdstNE3xg+r/tTayvSRzMHyoX/PYTr6FKtky4u2sjx/3wH+WLqMFvKzbiNSgbT/lRrGaS1FMxVR75/7qYchVCJieauSg==";
            //string aa = "15793";
            //string a1 = "8nDF24C2AD";
            //// string a2 = "1714373200";
            //string a2 = tStamp.ToString();
            //string a3 = aa + a1 + a2;
            //string LZDecrypted = Decrypt(a3, r);
            //string result = LZString.DecompressFromEncodedURIComponent(LZDecrypted);

            //Console.WriteLine("==========================================================================================");
        }

        public string Decrypt(string key, string data)
        {
            string decData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return decData;
        }

        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt =
                            new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        private byte[][] GetHashKeys(string key)
        //public byte[][] GetHashKeys(string key)
        {
            byte[][] result = new byte[2][];
            Encoding enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            byte[] rawKey = enc.GetBytes(key);
            byte[] rawIV = enc.GetBytes(key);

            byte[] hashKey = sha2.ComputeHash(rawKey);
            byte[] hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        private void HandleInvalidSubmit()
        {
            FormValidationState = false;
        }

        private void OnCancel()
        {
            InsurancePoliciyForm = new();
            ShowForm = false;
        }

        #endregion Form
    }
}