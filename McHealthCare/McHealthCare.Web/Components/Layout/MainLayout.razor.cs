namespace McHealthCare.Web.Components.Layout
{
    public partial class MainLayout
    {
        private bool showPreloader = true;

        protected override async Task OnInitializedAsync()
        {
            showPreloader = false;
        }
    }
}