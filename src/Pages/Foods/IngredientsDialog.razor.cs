using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Restaurant.Template.Pages.Foods;

public partial class IngredientsDialog : ComponentBase
{
    #region Global Properties & Variables
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }
    
    //private DynamicMudForm _dynamicMudFrom;

    #endregion
    
    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        //await JsonFormBuilder();
    }

    #endregion

   

    #region Event Mapper
    async Task SubmitButton_Click(EventArgs args)
    {
        Logger.LogInformation("Submit Button Click");
        //await _dynamicMudFrom.GetMudFrom().Validate();
        //if (_dynamicMudFrom.GetMudFrom().IsValid)
        {
            //Logger.LogInformation("FormData {FormData}", _dynamicMudFrom.GetFormData()); 
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
    
    async Task CancelAction()
    {
        Logger.LogInformation("Cancel MudBlazor MudDialog click");
        MudDialog.Cancel();
    }
    #endregion
}