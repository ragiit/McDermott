namespace McDermott.Web.Extentions
{
    public class MyDxComboBox<TData, TValue> : DxComboBox<TData, TValue>
    {
        private bool _initialParametersSet;

        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                NullText = "Select Name...";
                ShowValidationIcon = true;
                ClearButtonDisplayMode = DataEditorClearButtonDisplayMode.Auto;
                //FilteringMode = DataGridFilteringMode.Contains;
                ListRenderMode = ListRenderMode.Virtual;
                AllowUserInput = true;
                ShowValidationIcon = true;
                _initialParametersSet = true;
            }
            return base.SetParametersAsyncCore(parameters);
        }
    }
}