using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Restaurant.Template.Dialogs;

public partial class GeneralSettingsDialog : ComponentBase
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

    private string _cardTitle = "general-settings";
    private async Task JsonFormBuilder()
    {
        #region All Controls
        _formBuilders = await Http.GetFromJsonAsync<FormBuilder[]>($"forms/dialog/{_cardTitle}.json?v={DateTime.Now.Ticks}" );
    
        if (_formBuilders is not null)
        {
            DynamicMudForm.AttachCard_EventAction(_formBuilders, _cardTitle, CardActionClick);
            DynamicMudForm.AttachSubmitButton_EventAction(_formBuilders, _cardTitle, SubmitButton_Click);
            DynamicMudForm.AttachCancelButton_EventAction(_formBuilders, _cardTitle, CancelAction);
            
        }

        #endregion
        StateHasChanged();
    }

    #endregion

    #region Event Mapper

    private void AttachValidationEvent()
    {
        var detailPanel = _formBuilders.FirstOrDefault(card => card.Card == _cardTitle);
        if (detailPanel is not null)
        {
            var field = detailPanel.Fields.FirstOrDefault(field => field.FieldName == "AppURL");
            if (field is not null)
            {
                field.Validation.Method = new UrlAttribute() { ErrorMessage = "The URL is not valid" };
            }
        }
    }
    public async Task CardActionClick()
    {
        Console.WriteLine("Triggered the TextBox Card Action Clicked.");
    }

    public async Task SubmitButton_Click(EventArgs args)
    {
        Logger.LogInformation("Submit Button Click");
        await _dynamicMudFrom.GetMudFrom().Validate();
        if (_dynamicMudFrom.GetMudFrom().IsValid)
        {
            Logger.LogInformation("FormData {FormData}", _dynamicMudFrom.GetFormData()); 
        }

        MudDialog.Close(DialogResult.Ok(true));

    }
    
    public async Task CancelAction()
    {
        Logger.LogInformation("Cancel MudBlazor MudDialog click");
        MudDialog.Cancel();
    }
    #endregion
}