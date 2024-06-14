namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class AccidentPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private (bool, GroupMenuDto, User) Test = new();
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

        private IEnumerable<string> D1 { get; set; } = [
      "Foreign body",
    "Eye irritation"
  ];

        private IEnumerable<string> D2 { get; set; } = [
            "Abrasion",
    "Laceration",
    "Puncture",
    "Scratch"
        ];

        private IEnumerable<string> D3 { get; set; } = [
            "Bruise",
    "Contussion",
    "Crushing"
        ];

        private IEnumerable<string> D4 { get; set; } = [
            "Sprain"
        ];

        private IEnumerable<string> D5 { get; set; } = [
            "Fracture",
    "Dislocation"
        ];

        private IEnumerable<string> D6 { get; set; } = [
            "Burn",
    "Chemical burn",
    "Electric burn"
        ];

        private IEnumerable<string> D7 { get; set; } = [
            "Occupational illness",
    "LBP",
    "Dermatitis"
        ];

        private IEnumerable<string> D8 { get; set; } = [
            "Asphyxia",
    "Intoxication",
    "Amputation",
    "Concussion"
        ];

        private IEnumerable<string> D9 { get; set; } = [
    "Head",
    "face",
    "neck"
];

        private IEnumerable<string> D10 { get; set; } = [
            "Eye"
        ];

        private IEnumerable<string> D11 { get; set; } = [
            "Ear"
        ];

        private IEnumerable<string> D12 { get; set; } = [
            "Back"
        ];

        private IEnumerable<string> D13 { get; set; } = [
            "Trunk (except back and internal)"
        ];

        private IEnumerable<string> D14 { get; set; } = [
            "Arm"
        ];

        private IEnumerable<string> D15 { get; set; } = [
            "Hand and wrist"
        ];

        private IEnumerable<string> D16 { get; set; } = [
            "Fingers"
        ];

        private IEnumerable<string> D17 { get; set; } = [
            "Leg"
        ];

        private IEnumerable<string> D18 { get; set; } = [
            "Feet and ankles"
        ];

        private IEnumerable<string> D19 { get; set; } = [
            "Toes"
        ];

        private IEnumerable<string> D20 { get; set; } = [
            "Internal and Others"
        ];

        private IEnumerable<string> D21 { get; set; } = [
    "Falls",
    "Slips",
    "Trips"
];

        private IEnumerable<string> D22 { get; set; } = [
            "Fire",
    "hot materials"
        ];

        private IEnumerable<string> D23 { get; set; } = [
            "Pressurized gas"
        ];

        private IEnumerable<string> D24 { get; set; } = [
            "Foreign body"
        ];

        private IEnumerable<string> D25 { get; set; } = [
            "Electricity"
        ];

        private IEnumerable<string> D26 { get; set; } = [
            "Sandblast"
        ];

        private IEnumerable<string> D27 { get; set; } = [
            "Animal",
    "plant"
        ];

        private IEnumerable<string> D28 { get; set; } = [
            "Struck",
    "caught (by, against, between) objects"
        ];

        private IEnumerable<string> D29 { get; set; } = [
            "Chemicals"
        ];

        private IEnumerable<string> D30 { get; set; } = [
            "Welding flash"
        ];

        private IEnumerable<string> D31 { get; set; } = [
            "Vehicle accident"
        ];

        private IEnumerable<string> D32 { get; set; } = [
            "Overexertion"
        ];

        private IEnumerable<string> D33 { get; set; } = [
            "Smoke",
    "gas"
        ];

        private IEnumerable<string> D34 { get; set; } = [
            "Others"
        ];

        private IEnumerable<string> S1 { get; set; } = [];
        private IEnumerable<string> S2 { get; set; } = [];
        private IEnumerable<string> S3 { get; set; } = [];
        private IEnumerable<string> S4 { get; set; } = [];
        private IEnumerable<string> S5 { get; set; } = [];
        private IEnumerable<string> S6 { get; set; } = [];
        private IEnumerable<string> S7 { get; set; } = [];
        private IEnumerable<string> S8 { get; set; } = [];
        private IEnumerable<string> S9 { get; set; } = [];
        private IEnumerable<string> S10 { get; set; } = [];
        private IEnumerable<string> S11 { get; set; } = [];
        private IEnumerable<string> S12 { get; set; } = [];
        private IEnumerable<string> S13 { get; set; } = [];
        private IEnumerable<string> S14 { get; set; } = [];
        private IEnumerable<string> S15 { get; set; } = [];
        private IEnumerable<string> S16 { get; set; } = [];
        private IEnumerable<string> S17 { get; set; } = [];
        private IEnumerable<string> S18 { get; set; } = [];
        private IEnumerable<string> S19 { get; set; } = [];
        private IEnumerable<string> S20 { get; set; } = [];
        private IEnumerable<string> S21 { get; set; } = [];
        private IEnumerable<string> S22 { get; set; } = [];
        private IEnumerable<string> S23 { get; set; } = [];
        private IEnumerable<string> S24 { get; set; } = [];
        private IEnumerable<string> S25 { get; set; } = [];
        private IEnumerable<string> S26 { get; set; } = [];
        private IEnumerable<string> S27 { get; set; } = [];
        private IEnumerable<string> S28 { get; set; } = [];
        private IEnumerable<string> S29 { get; set; } = [];
        private IEnumerable<string> S30 { get; set; } = [];
        private IEnumerable<string> S31 { get; set; } = [];
        private IEnumerable<string> S32 { get; set; } = [];
        private IEnumerable<string> S33 { get; set; } = [];
        private IEnumerable<string> S34 { get; set; } = [];

        #endregion UserLoginAndAccessRole

        private string StagingText = EnumStatusAccident.RestrictedWorkCase.GetDisplayName();

        private bool PanelVisible { get; set; } = true;
        private bool ShowForm { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private AccidentDto Accident { get; set; } = new();
        private List<AccidentDto> Data = [];

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            // Initialize S with D values
            S1 = D1.Select(x => x).AsEnumerable();
            S2 = D2.Select(x => x).AsEnumerable();
            S3 = D3.Select(x => x).AsEnumerable();
            S4 = D4.Select(x => x).AsEnumerable();
            S5 = D5.Select(x => x).AsEnumerable();
            S6 = D6.Select(x => x).AsEnumerable();
            S7 = D7.Select(x => x).AsEnumerable();
            S8 = D8.Select(x => x).AsEnumerable();
            S9 = D9.Select(x => x).AsEnumerable();
            S10 = D10.Select(x => x).AsEnumerable();
            S11 = D11.Select(x => x).AsEnumerable();
            S12 = D12.Select(x => x).AsEnumerable();
            S13 = D13.Select(x => x).AsEnumerable();
            S14 = D14.Select(x => x).AsEnumerable();
            S15 = D15.Select(x => x).AsEnumerable();
            S16 = D16.Select(x => x).AsEnumerable();
            S17 = D17.Select(x => x).AsEnumerable();
            S18 = D18.Select(x => x).AsEnumerable();
            S19 = D19.Select(x => x).AsEnumerable();
            S20 = D20.Select(x => x).AsEnumerable();
            S21 = D21.Select(x => x).AsEnumerable();
            S22 = D22.Select(x => x).AsEnumerable();
            S23 = D23.Select(x => x).AsEnumerable();
            S24 = D24.Select(x => x).AsEnumerable();
            S25 = D25.Select(x => x).AsEnumerable();
            S26 = D26.Select(x => x).AsEnumerable();
            S27 = D27.Select(x => x).AsEnumerable();
            S28 = D28.Select(x => x).AsEnumerable();
            S29 = D29.Select(x => x).AsEnumerable();
            S30 = D30.Select(x => x).AsEnumerable();
            S31 = D31.Select(x => x).AsEnumerable();
            S32 = D32.Select(x => x).AsEnumerable();
            S33 = D33.Select(x => x).AsEnumerable();
            S34 = D34.Select(x => x).AsEnumerable();

            await GetUserInfo();
            await LoadData();
        }

        public async Task OnSaving(GridEditModelSavingEventArgs e)
        {
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            Accident = new();
            ShowForm = false;
            PanelVisible = false;
        }

        private async Task OnClickBack()
        {
            await LoadData();
        }

        public async Task OnDeleting(GridDataItemDeletingEventArgs e)
        {
        }

        private async Task ExportToExcel()
        {
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            ShowForm = true;
            Accident = new();
            Accident.Id = 1;
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
    }
}