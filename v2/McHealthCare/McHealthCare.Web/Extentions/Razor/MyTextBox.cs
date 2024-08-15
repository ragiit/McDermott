namespace McHealthCare.Web.Extentions.Razor
{
    public class MyTextBox : DxTextBox
    {
        private bool _initialParametersSet;

        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                ShowValidationIcon = true;
                //NullText = "Name";
                ClearButtonDisplayMode = DataEditorClearButtonDisplayMode.Auto;
                _initialParametersSet = true;
            }
            return base.SetParametersAsyncCore(parameters);
        }
    }
}