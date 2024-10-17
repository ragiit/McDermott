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

                IsBPJS = Insurances.Any(x => x.IsBPJSKesehatan == true && x.Id == value) ? true : false;
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
                var user = await UserInfoService.GetUserInfo(ToastService);
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
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Grid Properties

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            Insurances = (await Mediator.Send(new GetInsuranceQuery())).Item1;

            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData(bool v = false)
        {
            PanelVisible = true;
            ShowForm = false;
            SelectedDataItems = [];

            var countries = await Mediator.Send(new GetCountryQuery());
            Countries = countries.Item1;
            InsurancePolicies = (await Mediator.Send(new GetInsurancePolicyQuery())).Item1;

            if (User != null && User.Id != 0)
            {
                InsurancePolicies = InsurancePolicies.Where(x => x.UserId == User.Id).ToList();
            }

            if (v)
                User = new() { Name = "-" };

            PanelVisible = false;
        }

        #endregion LoadData

        #region Grid Function

        private void SelectedItemInsuranceChanged(InsuranceDto e)
        {
            if (e is null)
                return;

            IsBPJS = Insurances.Any(x => x.IsBPJSKesehatan == true && x.Id == e.Id);
            var ee = IsBPJS;
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
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
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

                if (User is null || User.Id == 0)
                {
                    ToastService.ShowInfo("Please select the Patient first.");
                    return;
                }

                InsurancePoliciyForm.UserId = User.Id;

                {
                    //var query = BPJSIntegration.Id == 0
                    //            ? new GetBPJSIntegrationQuery(x => x.Id != BPJSIntegration.Id && !string.IsNullOrWhiteSpace(x.NoKartu) && x.NoKartu.ToLower().Trim().Equals(BPJSIntegration.NoKartu))
                    //            : new GetBPJSIntegrationQuery(x => x.Id != BPJSIntegration.Id && !string.IsNullOrWhiteSpace(x.NoKartu) && x.NoKartu.ToLower().Trim().Equals(BPJSIntegration.NoKartu));

                    if (IsBPJS)
                    {
                        if (BPJSIntegration.NoKartu != InsurancePoliciyForm.PolicyNumber)
                        {
                            ToastService.ShowInfo("Card Number & Policy Number must be same");
                            return;
                        }
                    }

                    //var query = new GetBPJSIntegrationQuery(x => x.Id != BPJSIntegration.Id && !string.IsNullOrWhiteSpace(x.NoKartu) && x.NoKartu.ToLower().Trim().Equals(BPJSIntegration.NoKartu));
                    //var bpjs = await Mediator.Send(query);
                    //if (bpjs.Count > 0)
                    //{
                    //    ToastService.ShowInfo("Card Number already exist");
                    //    return;
                    //}
                }

                if (InsurancePoliciyForm.Id == 0)
                {
                    InsurancePoliciyForm = await Mediator.Send(new CreateInsurancePolicyRequest(InsurancePoliciyForm));

                    if (IsBPJS)
                    {
                        var insurance = Insurances.FirstOrDefault(x => x.Name.Contains("BPJS Kesehatan"));

                        //await Mediator.Send(new DeleteBPJSIntegrationRequest(DeletedBPJSID));

                        //if (insurance is not null && InsurancePoliciyForm.InsuranceId == insurance.Id)
                        //{
                        //    BPJSIntegration.InsurancePolicyId = InsurancePoliciyForm.Id;
                        //    await Mediator.Send(new CreateBPJSIntegrationRequest(BPJSIntegration));
                        //}
                    }
                }
                else
                {
                    await Mediator.Send(new UpdateInsurancePolicyRequest(InsurancePoliciyForm));

                    if (IsBPJS)
                    {
                        var insurance = Insurances.FirstOrDefault(x => x.Name.Contains("BPJS Kesehatan"));

                        //var cek = await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == InsurancePoliciyForm.Id));

                        //if (insurance is not null && cek is not null && cek.Count > 0)
                        //{
                        //    //BPJSIntegration = cek.Adapt<BPJSIntegrationDto>();
                        //    cek.Adapt(BPJSIntegration); // Adapt cek to BPJSIntegration
                        //    var aa = BPJSIntegration;
                        //    BPJSIntegration.Id = cek[0].Id;
                        //    BPJSIntegration.InsurancePolicyId = cek[0].InsurancePolicyId;
                        //    await Mediator.Send(new UpdateBPJSIntegrationRequest(BPJSIntegration));
                        //}
                        //else
                        //{
                        //    await Mediator.Send(new DeleteBPJSIntegrationRequest(DeletedBPJSID));
                        //    BPJSIntegration.InsurancePolicyId = InsurancePoliciyForm.Id;
                        //    await Mediator.Send(new CreateBPJSIntegrationRequest(BPJSIntegration));
                        //}
                    }//if (DeletedBPJSID != 0 && !IsBPJS)
                    //{
                    //    await Mediator.Send(new DeleteBPJSIntegrationRequest(DeletedBPJSID));
                    //}
                    //else
                    //{
                    //    if (insurance is not null && InsurancePoliciyForm.InsuranceId == insurance.Id)
                    //    {
                    //        if (IsBPJS && BPJSIntegration.Id != 0)
                    //        {
                    //            await Mediator.Send(new UpdateBPJSIntegrationRequest(BPJSIntegration));
                    //        }
                    //    }
                    //}
                }

                DeletedBPJSID = 0;
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
            BPJSIntegration = new();
            IsBPJS = false;
            DeletedBPJSID = 0;
            ShowForm = true;
        }

        private long DeletedBPJSID { get; set; } = 0;

        private async Task EditItem_Click()
        {
            try
            {
                InsurancePoliciyForm = new();
                BPJSIntegration = new();
                DeletedBPJSID = 0;
                InsurancePoliciyForm = SelectedDataItems[0].Adapt<InsurancePolicyDto>();
                //IsBPJS = Insurances.Any(x => x.IsBPJSKesehatan == true && x.Id == InsurancePoliciyForm.InsuranceId);
                //if (IsBPJS)
                //{
                //    var BPJSIntegration = await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == InsurancePoliciyForm.Id));
                //    if (BPJSIntegration.Count > 0)
                //    {
                //        this.BPJSIntegration = BPJSIntegration[0];
                //        DeletedBPJSID = BPJSIntegration[0].Id;
                //    }
                //}
                User = InsurancePoliciyForm.User.Adapt<UserDto>();
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
        private ResponseAPIBPJSIntegrationGetPeserta ResponseAPIBPJSIntegrationGetPeserta { get; set; } = new();
        private InsurancePolicyDto BPJSIntegration { get; set; } = new();

        private async Task OnClickGetBPJS()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InsurancePoliciyForm.PolicyNumber))
                {
                    ToastService.ShowInfo("Please insert the Policy Number!");
                    return;
                }

                IsLoadingGetBPJS = true;

                var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"peserta/{InsurancePoliciyForm.PolicyNumber}", HttpMethod.Get);
                if (result.Item2 == 200)
                {
                    if (result.Item1 == null)
                    {
                        ResponseAPIBPJSIntegrationGetPeserta = new();
                        BPJSIntegration = new();
                        IsLoadingGetBPJS = false;
                        return;
                    }

                    ResponseAPIBPJSIntegrationGetPeserta = System.Text.Json.JsonSerializer.Deserialize<ResponseAPIBPJSIntegrationGetPeserta>(result.Item1);
                    BPJSIntegration = ResponseAPIBPJSIntegrationGetPeserta.Adapt<InsurancePolicyDto>();

                    BPJSIntegration.AsuransiNoAsuransi = ResponseAPIBPJSIntegrationGetPeserta.Asuransii.NoAsuransi;
                    BPJSIntegration.AsuransiNmAsuransi = ResponseAPIBPJSIntegrationGetPeserta.Asuransii.NmAsuransi;
                    BPJSIntegration.AsuransiCob = ResponseAPIBPJSIntegrationGetPeserta.Asuransii.Cob;
                    BPJSIntegration.AsuransiKdAsuransi = ResponseAPIBPJSIntegrationGetPeserta.Asuransii.KdAsuransi;

                    BPJSIntegration.JnsKelasNama = ResponseAPIBPJSIntegrationGetPeserta.JnsKelass.Nama;
                    BPJSIntegration.JnsKelasKode = ResponseAPIBPJSIntegrationGetPeserta.JnsKelass.Kode;

                    BPJSIntegration.JnsPesertaNama = ResponseAPIBPJSIntegrationGetPeserta.JnsPesertaa.Nama;
                    BPJSIntegration.JnsPesertaKode = ResponseAPIBPJSIntegrationGetPeserta.JnsPesertaa.Kode;

                    BPJSIntegration.KdProviderGigiKdProvider = ResponseAPIBPJSIntegrationGetPeserta.KdProviderGigii.KdProvider;
                    BPJSIntegration.KdProviderGigiNmProvider = ResponseAPIBPJSIntegrationGetPeserta.KdProviderGigii.NmProvider;

                    BPJSIntegration.KdProviderPstKdProvider = ResponseAPIBPJSIntegrationGetPeserta.KdProviderPstt.KdProvider;
                    BPJSIntegration.KdProviderPstNmProvider = ResponseAPIBPJSIntegrationGetPeserta.KdProviderPstt.NmProvider;
                }
                else
                {
                    ResponseAPIBPJSIntegrationGetPeserta = new();
                    BPJSIntegration = new();

                    dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                    ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                }

                IsLoadingGetBPJS = false;
            }
            catch (Exception ex)
            {
                IsLoadingGetBPJS = false;
                ex.HandleException(ToastService);
            }

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
            User = new();
            ShowForm = false;
        }

        #endregion Form
    }
}