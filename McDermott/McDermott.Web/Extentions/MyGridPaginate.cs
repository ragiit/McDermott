namespace McDermott.Web.Extentions
{
    public class MyGridPaginate : DxGrid
    {
        private bool _initialParametersSet;

        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                PagerNavigationMode = PagerNavigationMode.InputBox;
                EditorRenderMode = GridEditorRenderMode.Detached;
                PageSize = 100;
                FilterMenuButtonDisplayMode = GridFilterMenuButtonDisplayMode.Always;
                CustomizeDataRowEditor = @GridExtention.Grid_CustomizeDataRowEditor;
                AllowSelectRowByClick = true;
                PagerPosition = GridPagerPosition.Bottom;
                PageSizeSelectorVisible = true;
                PageSizeSelectorItems = ([10, 20, 50, 100]);
                PagerSwitchToInputBoxButtonCount = 10;
                PagerVisible = false;
                PageSizeSelectorVisible = false;
                FocusedRowEnabled = true;
                TextWrapEnabled = false;
                ValidationEnabled = true;
                ColumnResizeMode = GridColumnResizeMode.NextColumn;
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