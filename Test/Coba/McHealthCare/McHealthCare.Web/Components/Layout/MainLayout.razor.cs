using Microsoft.AspNetCore.Components.Routing;

namespace McHealthCare.Web.Components.Layout
{
    public partial class MainLayout
    {
        private bool showPreloader = true;
        private string currentNav;

        protected override void OnInitialized()
        {
            currentNav = NavigationManager.Uri;
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            currentNav = e.Location;
            InvokeAsync(StateHasChanged);
        }

        private string IsActiveNav(string navItem)
        {
            if (navItem.Contains("home"))
                return "active";

            var a = currentNav.Contains(navItem) ? "active" : "";

            return a;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        protected override async Task OnInitializedAsync()
        {
            showPreloader = false;
        }
    }
}