namespace McDermott.Web.Extentions
{
    public class _BaseMyGrid : DxGrid
    {
        private bool _initialParametersSet;

        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                ShowGroupPanel = true;
                VirtualScrollingEnabled = false;
                AutoExpandAllGroupRows = true;
                KeyboardNavigationEnabled = true;
                FocusedRowEnabled = true;
                ValidationEnabled = true;
                AllowSelectRowByClick = true;
                PageSizeSelectorVisible = true;
                ShowSearchBox = true;
                ShowFilterRow = true;
                TextWrapEnabled = false;

                PageSizeSelectorItems = ([10, 20, 50, 100, 200]);
                PageSize = 20;
                PagerSwitchToInputBoxButtonCount = 10;
                PagerVisibleNumericButtonCount = 10;

                PagerPosition = GridPagerPosition.Bottom;
                FilterMenuButtonDisplayMode = GridFilterMenuButtonDisplayMode.Always;
                ColumnResizeMode = GridColumnResizeMode.NextColumn;
                CustomizeDataRowEditor = @GridExtention.Grid_CustomizeDataRowEditor;
                CustomizeElement = @GridExtention.Grid_CustomizeElement;
                PagerNavigationMode = PagerNavigationMode.InputBox;
                EditorRenderMode = GridEditorRenderMode.Detached;
            }

            return base.SetParametersAsyncCore(parameters);
        }
    }
}