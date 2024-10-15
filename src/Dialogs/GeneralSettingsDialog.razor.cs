using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Reflection;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using LiteDB;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Model = Restaurant.Template.Models;

namespace Restaurant.Template.Dialogs;

public partial class GeneralSettingsDialog : ComponentBase
{
    #region Global Properties & Variables
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }
    
    private DynamicMudForm _dynamicMudFrom;
    
    private ILiteCollection<Model.GeneralSettings> _generalSettingsCollection;
    private Model.GeneralSettings _generalSettings = new Model.GeneralSettings();

    #endregion
    
    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        _generalSettingsCollection =  LiteDb.GetCollection<Model.GeneralSettings>();
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
            DynamicMudForm.AttachValidationEvent(_formBuilders,
                _cardTitle,
                "AppURL",
                new UrlAttribute() { ErrorMessage = "The URL is not valid" });
            var detailPanel = _formBuilders.FirstOrDefault(card => card.Card == _cardTitle);
            
            _generalSettings = await GetSettings();
            SetPropertyValues(detailPanel, _generalSettings);
        }

        #endregion
        StateHasChanged();
    }

    private void SetPropertyValues(FormBuilder? detailPanel, Model.GeneralSettings
         settings)
    {
        if (settings is null)
        {
            Logger.LogWarning("'settings' object, is Null");
            return;
        }
        var type = settings.GetType();
        var props = type.GetProperties(
            BindingFlags.Public | 
                      BindingFlags.Instance);
        foreach (var prop in props)
        {
            //Logger.LogInformation($"{prop.Name}: {value}");
            var field =  detailPanel.Fields.Where(f => f.FieldName.Equals(prop.Name)).FirstOrDefault();
            if (field is null) continue;
            if (field.FieldName != prop.Name) continue;
            field.DefaultValue = prop.GetValue(settings).ToString();
            Logger.LogInformation($"Field Name : {prop.Name}: Default Value:  {field.DefaultValue}");
        }
    }

    #endregion

    #region Event Mapper
    async Task CardActionClick()
    {
        Console.WriteLine("Triggered the TextBox Card Action Clicked.");
    }

    async Task SubmitButton_Click(EventArgs args)
    {
        Logger.LogInformation("Submit Button Click");
        try
        {
            await _dynamicMudFrom.GetMudFrom().Validate();
            if (_dynamicMudFrom.GetMudFrom().IsValid)
            {
                Logger.LogInformation("FormData {FormData}", _dynamicMudFrom.GetFormData()); 
                await InsertSettings();
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        finally
        {
            await LiteDb.DisposeAsync();
        }
        
    }

    
    async Task CancelAction()
    {
        Logger.LogInformation("Cancel MudBlazor MudDialog click");
        MudDialog.Cancel();
    }
    #endregion

    #region DB Actions

    async Task<Model.GeneralSettings> GetSettings()
    {
        await LiteDb.OpenAsync();
        var settings = await _generalSettingsCollection.Query().FirstOrDefaultAsync();
        return settings;
    }

    async Task InsertSettings()
    {
        await LiteDb.OpenAsync();
        var settings = System.Text.Json.JsonSerializer.Deserialize<Model.GeneralSettings>(_dynamicMudFrom.GetFormData());
        if (settings is not null)
        {
            await _generalSettingsCollection.DeleteAllAsync();//everytime only one data should be present.
            await _generalSettingsCollection.InsertAsync(settings);
            //Expenses<Func<Model.GeneralSettings, bool>> x;
            _generalSettingsCollection.Query().Where(x => x.IsSync == true);
            await LiteDb.CheckpointAsync();
            Logger.LogInformation("General Settings 'Saved' using LiteDB");
            Logger.LogInformation(settings.ToString());
            
        }
    }


    #endregion
}