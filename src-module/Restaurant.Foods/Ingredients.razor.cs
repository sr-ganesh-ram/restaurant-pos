using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Restaurant.Foods;

public partial class Ingredients : ComponentBase
{
    #region Property

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
        }

        #endregion
        StateHasChanged();
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
        }
        
    }
    async Task CancelAction()
    {
        Logger.LogInformation("Cancel MudBlazor MudDialog click");
    }
    #endregion
}