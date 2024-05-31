namespace McDermott.Web.Extentions
{
    public static class GridExtention
    {
        public static void Grid_CustomizeElement(this GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
        }

        public static void Grid_CustomizeDataRowEditor(this GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        public static void ColumnChooserButton_Click(this IGrid grid)
        {
            grid.ShowColumnChooser();
        }

        public static async Task ExportXlsxItem_Click(this IGrid grid)
        {
            await grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        public static async Task ExportXlsItem_Click(this IGrid grid)
        {
            await grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        public static async Task ExportCsvItem_Click(this IGrid grid)
        {
            await grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }
    }
}
