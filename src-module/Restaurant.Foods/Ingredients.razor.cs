using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using LiteDB;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Restaurant.DataAccess;
using Model = Restaurant.DataAccess.Entities;
using MudBlazor;

namespace Restaurant.Foods;

public partial class Ingredients : ComponentBase
{
    #region Property

    private DynamicMudForm _dynamicMudFrom;
    [Parameter]
    public MudDialogInstance MudDialog { get; set; }
    
    [Parameter] 
    public required ILiteDatabase LiteDatabase { get; set; }

    #endregion
    
    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        await JsonFormBuilder();
    }
    #endregion

    #region Control Details

    private FormBuilder[]? _formBuilders;
    private string _cardTitle = "ingredient";
    private async Task JsonFormBuilder()
    {
        #region All Controls
        _formBuilders = await Http.GetFromJsonAsync<FormBuilder[]>($"_content/Restaurant.Foods/forms/{_cardTitle}.json?v={DateTime.Now.Ticks}" );
    
        if (_formBuilders is not null)
        {
            DynamicMudForm.AttachCard_EventAction(_formBuilders, _cardTitle, CardActionClick);
            DynamicMudForm.AttachSubmitButton_EventAction(_formBuilders, _cardTitle, SubmitButton_Click);
            DynamicMudForm.AttachCancelButton_EventAction(_formBuilders, _cardTitle, CancelAction);
            //var fieldUrl = GetField(_formBuilders, _cardTitle, "IconURL");
            /*var fieldPreview = GetField(_formBuilders, _cardTitle, "IconPreview");
            Logger.LogInformation($"FieldURL {fieldUrl.FieldName} Field Preview {fieldPreview.FieldName}");
             fieldUrl.OnBlur = async args =>
             {
                 Logger.LogInformation($"Url {fieldUrl.DefaultValue} ");
                 fieldPreview.ImageSrc = fieldUrl.DefaultValue;
                 StateHasChanged();
             }; */
        }

        #endregion
        StateHasChanged();
    }
    Field? GetField(FormBuilder[] formBuilders, string cardName, string fieldName)
    {
        Field? result = null;
        var cardPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (cardPanel is null) return result;
        var field = cardPanel.Fields.FirstOrDefault(field => field.FieldName == fieldName);
        result = field;
        return result;
    }
    private Task OnBlurx(EventCallback<FocusEventArgs> arg)
    {
        return Task.CompletedTask;
    }

    #endregion

    #region Event Mapper
    async Task CardActionClick()
    {
        Console.WriteLine("Triggered the Card Action Clicked.");
    }
    async Task SubmitButton_Click(EventArgs args)
    {
        Logger.LogInformation("Submit Button Click");
        await _dynamicMudFrom.GetMudFrom().Validate();
        if (_dynamicMudFrom.GetMudFrom().IsValid)
        {
            Logger.LogInformation("FormData {FormData}", _dynamicMudFrom.GetFormData()); 
            
            DBAccess<Model.Ingredients> dbAccess = 
                new DBAccess<Model.Ingredients>(LiteDatabase, logger:Logger);
            var data = System.Text.Json.JsonSerializer.Deserialize<Model.Ingredients>(_dynamicMudFrom.GetFormData());
            var result = await dbAccess.Insert(data);
            if (result)
            {
                Logger.LogInformation("Ingredients Data Inserted");
                Snackbar.Add("Ingredients Data Inserted", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                Snackbar.Add("Something went wrong see console for more details.", Severity.Error);
            }
        }
        
    }
    async Task CancelAction()
    {
        Logger.LogInformation("Cancel MudBlazor MudDialog click");
        if (MudDialog is not null)
        {
            MudDialog.Cancel();
        }
    }
    #endregion
}