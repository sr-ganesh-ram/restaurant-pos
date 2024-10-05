using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Restaurant.Template.Dialogs;

public partial class InvoicePrintingDialog : ComponentBase
{
    
    #region Global Properties & Variables
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }
    
    private DynamicMudForm _dynamicMudFrom;

    #endregion
    
    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        await JsonFormBuilder();
    }

    #endregion

    #region Control Details

    private FormBuilder[]? _formBuilders;

    private string _cardTitle = "invoice-printing";
    private async Task JsonFormBuilder()
    {
        #region All Controls
        _formBuilders = await Http.GetFromJsonAsync<FormBuilder[]>($"forms/dialog/{_cardTitle}.json?v={DateTime.Now.Ticks}" );
    
        if (_formBuilders is not null)
        {
            DynamicMudForm.AttachSubmitButton_EventAction(_formBuilders, _cardTitle, SubmitButton_Click);
            DynamicMudForm.AttachCancelButton_EventAction(_formBuilders, _cardTitle, CancelAction);
            DynamicMudForm.AttachValidationEvent(_formBuilders,
                _cardTitle,
                "AppURL",
                new UrlAttribute() { ErrorMessage = "The URL is not valid" });
        }

        #endregion
        StateHasChanged();
    }

    #endregion

    #region Event Mapper
    async Task SubmitButton_Click(EventArgs args)
    {
        Logger.LogInformation("Submit Button Click");
        await _dynamicMudFrom.GetMudFrom().Validate();
        if (_dynamicMudFrom.GetMudFrom().IsValid)
        {
            Logger.LogInformation("FormData {FormData}", _dynamicMudFrom.GetFormData()); 
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