using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp;
using DxBlazorApplication2.Module.BusinessObjects;

namespace DxBlazorApplication2.Blazor.Server.Controllers
{
    public partial class DateEditCalendarController : ObjectViewController<DetailView, Employee>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            //Access the Birthday property editor settings
            View.CustomizeViewItemControl<DateTimePropertyEditor>(this, SetCalendarView, nameof(Employee.Birthday));
        }

        private void SetCalendarView(DateTimePropertyEditor propertyEditor)
        {
            //Set the date picker display mode to scroll picker
            propertyEditor.ComponentModel.PickerDisplayMode = DevExpress.Blazor.DatePickerDisplayMode.ScrollPicker;
        }
    }
}