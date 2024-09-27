using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Restaurant.Template.Pages.Settings;

public partial class Home : ComponentBase
{
    async Task NavigateClicked(string navigateTo)
    {
        Navigation.NavigateTo($"/settings/{navigateTo}");
    }
    protected override void OnInitialized()
    {
        Navigation.LocationChanged += HandleLocationChanged;
    }

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        Logger.LogInformation("URL of new location: {Location}", e.Location);
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= HandleLocationChanged;
    }
}