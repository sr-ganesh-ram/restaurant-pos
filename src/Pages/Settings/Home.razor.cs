using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using Restaurant.Template.Dialogs;
using Restaurant.Template.Layout;

namespace Restaurant.Template.Pages.Settings;

public partial class Home : ComponentBase
{
    async Task<Task<IDialogReference>> NavigateClicked(string navigateTo)
    {
        //Navigation.NavigateTo($"/settings/{navigateTo}");
        var options = new DialogOptions { CloseOnEscapeKey = true };
        Task<IDialogReference> dialog = null;
        
        switch (navigateTo)
        {
            case "general-settings":
                dialog = DialogService.ShowAsync<GeneralSettingsDialog>("General Settings", options);
                break;
            case "outgoing-mail":
                dialog = DialogService.ShowAsync<OutgoingMailDialog>("Outgoing Emails", options);
                break;
            case "appearance":
                dialog = DialogService.ShowAsync<AppearanceDialog>("Appearance", options);
                break;
            case "currency":
                dialog = DialogService.ShowAsync<CurrencyDialog>("Currency", options);
                break;
        }

        return dialog;
        
    }
    protected override void OnInitialized()
    {
        Navigation.LocationChanged += HandleLocationChanged;
    }

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        DefaultLogger.LogInformation("URL of new location: {Location}", e.Location);
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= HandleLocationChanged;
    }
}