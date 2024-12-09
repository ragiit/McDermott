namespace McDermott.Web.Extentions
{
    public class _BaseMyComboBox<TData, TValue> : DxComboBox<TData, TValue>
    {
        private bool _initialParametersSet;

        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                NullText = "Select Name";
                ClearButtonDisplayMode = DataEditorClearButtonDisplayMode.Auto;
                FilteringMode = DataGridFilteringMode.Contains;
                ListRenderMode = ListRenderMode.Virtual;

                ShowValidationIcon = true;
                AllowUserInput = true;
                ShowValidationIcon = true;
                _initialParametersSet = true;
            }
            return base.SetParametersAsyncCore(parameters);
        }
    }
}