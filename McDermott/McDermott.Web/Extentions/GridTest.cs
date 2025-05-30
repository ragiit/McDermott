﻿namespace McDermott.Web.Extentions
{
    public class GridTest : DxGrid
    {
        private bool _initialParametersSet;

        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                PagerNavigationMode = PagerNavigationMode.InputBox;
                EditorRenderMode = GridEditorRenderMode.Detached;
                PageSize = 10;
                _initialParametersSet = true;
                CustomizeDataRowEditor = GridExtention.Grid_CustomizeDataRowEditor;
                AllowSelectRowByClick = true;
                PagerPosition = GridPagerPosition.Bottom;
                PageSizeSelectorVisible = true;
                PageSizeSelectorItems = [10, 20, 50, 100];
                PagerSwitchToInputBoxButtonCount = 10;
                FocusedRowEnabled = true;
                PagerVisibleNumericButtonCount = 10;
                ShowGroupPanel = true;
                AutoExpandAllGroupRows = true;
                ShowFilterRow = true;
                CustomizeElement = GridExtention.Grid_CustomizeElement;
                ShowSearchBox = true;
                SelectAllCheckboxMode = GridSelectAllCheckboxMode.Mixed;
                KeyboardNavigationEnabled = true;
            }
            return base.SetParametersAsyncCore(parameters);
        }
    }
}