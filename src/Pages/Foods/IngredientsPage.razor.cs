using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Restaurant.Template.Pages.Foods;

public partial class IngredientsPage : ComponentBase
{
    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        
    }
    #endregion
    
    private void AddIngredients(MouseEventArgs obj)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        Task<IDialogReference> dialog = null;
        dialog = DialogService.ShowAsync<IngredientsDialog>("Add Ingredients", options);
    }
}