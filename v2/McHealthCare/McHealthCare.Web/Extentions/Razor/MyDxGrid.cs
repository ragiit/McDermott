namespace McHealthCare.Web.Extentions.Razor
{
    public class MyDxGrid : DxGrid
    {
        public void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        public void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }

            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                //e.CssClass = "header-bold";
            }
        }

        private bool _initialParametersSet;

        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                PagerNavigationMode = PagerNavigationMode.InputBox;
                EditorRenderMode = GridEditorRenderMode.Detached;
                PageSize = 100;
                FilterMenuButtonDisplayMode = GridFilterMenuButtonDisplayMode.Always;
                CustomizeDataRowEditor = Grid_CustomizeDataRowEditor;
                AllowSelectRowByClick = true;
                PagerPosition = GridPagerPosition.Bottom;
                PageSizeSelectorVisible = true;
                PageSizeSelectorItems = (new int[] { 10, 20, 50, 100 });
                //PageSizeSelectorAllRowsItemVisible = true;
                PagerSwitchToInputBoxButtonCount = 10;
                FocusedRowEnabled = true;
                TextWrapEnabled = false;
                ValidationEnabled = true;
                ColumnResizeMode = GridColumnResizeMode.NextColumn;
                PagerVisibleNumericButtonCount = 10;
                ShowGroupPanel = true;
                AutoExpandAllGroupRows = true;
                ShowFilterRow = true;
                CustomizeElement = Grid_CustomizeElement;
                ShowSearchBox = true;
                SelectAllCheckboxMode = GridSelectAllCheckboxMode.Mixed;
            }
            return base.SetParametersAsyncCore(parameters);
        }
    }
}