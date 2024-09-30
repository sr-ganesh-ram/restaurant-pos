using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Blazor.Shared.FormGenerator;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Restaurant.Template.Dialogs;

public partial class AppearanceDialog
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

    private string _cardTitle = "appearance";
    private async Task JsonFormBuilder()
    {
        #region All Controls
        _formBuilders = await Http.GetFromJsonAsync<FormBuilder[]>($"forms/dialog/{_cardTitle}.json?v={DateTime.Now.Ticks}" );
    
        if (_formBuilders is not null)
        {
            DynamicMudForm.AttachCard_EventAction(_formBuilders, _cardTitle, CardActionClick);
            DynamicMudForm.AttachSubmitButton_EventAction(_formBuilders, _cardTitle, SubmitButton_Click);
            DynamicMudForm.AttachCancelButton_EventAction(_formBuilders, _cardTitle, CancelAction);
            
            DynamicMudForm.AttachFileUpload_EventAction(
                _formBuilders, 
                _cardTitle, 
                "Logo", 
                UploadPhotoFile1, Logger);
            
            DynamicMudForm.AttachFileUpload_EventAction(
                _formBuilders, 
                _cardTitle, 
                "Icon", 
                UploadPhotoFile2, Logger);
        }

        #endregion
        StateHasChanged();
    }

    #endregion

    #region Event Mapper

    #region Attachment Files Events
    
    private GlobalBuilderSettings _globalBuilderSettings = new GlobalBuilderSettings();
    private async Task UploadPhotoFile1(IBrowserFile file)
    {
        string key = "Logo";
        if (_globalBuilderSettings.SelectedFile.ContainsKey(key))
        {
            _globalBuilderSettings.SelectedFile.Remove(key);
        }
        _globalBuilderSettings.SelectedFile.Add(key, file);
    }
    private async Task UploadPhotoFile2(IBrowserFile file)
    {
        string key = "Icon";
        if (_globalBuilderSettings.SelectedFile.ContainsKey(key))
        {
            _globalBuilderSettings.SelectedFile.Remove(key);
        }
        _globalBuilderSettings.SelectedFile.Add(key, file);
    }

    #endregion
    async Task CardActionClick()
    {
        Console.WriteLine("Triggered the TextBox Card Action Clicked.");
    }
    async Task SubmitButton_Click(EventArgs args)
    {
        Logger.LogInformation("Submit Button Click");
        await _dynamicMudFrom.GetMudFrom().Validate();
        if (_dynamicMudFrom.GetMudFrom().IsValid)
        {
            Logger.LogInformation("FormData {FormData}", _dynamicMudFrom.GetFormData()); 
        }
        MudDialog.Close(DialogResult.Ok(true));
    }
    async Task CancelAction()
    {
        Logger.LogInformation("Cancel MudBlazor MudDialog click");
        MudDialog.Cancel();
    }
    #endregion
}