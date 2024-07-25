using Microsoft.AspNetCore.Components;

namespace McHealthCare.Web.Extentions.Razor
{
    public class MyDxComboBox<TData, TValue> : DxComboBox<TData, TValue>
    {
        bool _initialParametersSet;
        protected override Task SetParametersAsyncCore(ParameterView parameters)
        {
            if (!_initialParametersSet)
            {
                NullText = "Select Name...";
                ShowValidationIcon = true;
                ClearButtonDisplayMode = DataEditorClearButtonDisplayMode.Auto;
                FilteringMode = DataGridFilteringMode.Contains;
                ListRenderMode = ListRenderMode.Virtual;
                _initialParametersSet = true;
            }
            return base.SetParametersAsyncCore(parameters);
        }
    }
}
