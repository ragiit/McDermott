namespace McDermott.Web.Extentions
{
    public class MyGrid : DxGrid
    {
        bool _initialParametersSet;
        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                PagerNavigationMode = PagerNavigationMode.InputBox;
                EditorRenderMode = GridEditorRenderMode.Detached;
                PageSize = 10;
                FilterMenuButtonDisplayMode = GridFilterMenuButtonDisplayMode.Always;
                CustomizeDataRowEditor = @GridExtention.Grid_CustomizeDataRowEditor;
                AllowSelectRowByClick = true;
                PagerPosition = GridPagerPosition.Bottom;
                PageSizeSelectorVisible = true;
                PageSizeSelectorItems = (new int[] { 10, 20, 50, 100 });
                //PageSizeSelectorAllRowsItemVisible = true;
                PagerSwitchToInputBoxButtonCount = 10;
                FocusedRowEnabled = true;
                PagerVisibleNumericButtonCount = 10;
                ShowGroupPanel = true;
                AutoExpandAllGroupRows = true;
                ShowFilterRow = true;
                CustomizeElement = @GridExtention.Grid_CustomizeElement;
                ShowSearchBox = true;
                SelectAllCheckboxMode = GridSelectAllCheckboxMode.Mixed;
            }
            return base.SetParametersAsyncCore(parameters);
        }
    }
}
