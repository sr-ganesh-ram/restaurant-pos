using LiteDB;
using Microsoft.AspNetCore.Components;
using Restaurant.DataAccess;
using Model = Restaurant.DataAccess.Entities;

namespace Restaurant.Foods;

public partial class IngredientsView : ComponentBase
{
    public record Employee(string Name, string Position, int YearsEmployed, int Salary, int Rating);
    public IEnumerable<Employee> employees;
    [Parameter] 
    public required ILiteDatabase LiteDatabase { get; set; }

    private IEnumerable<Model.Ingredients> _ingredients;
    
    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        employees = new List<Employee>
        {
            new Employee("Sam", "CPA", 23, 87_000, 4),
            new Employee("Alicia", "Product Manager", 11, 143_000, 5),
            new Employee("Ira", "Developer", 4, 92_000, 3),
            new Employee("John", "IT Director", 17, 229_000, 4),
        };
        
        DBAccess<Model.Ingredients> dbAccess = 
            new DBAccess<Model.Ingredients>(LiteDatabase, logger:Logger);
        _ingredients = await dbAccess.GetAll();
    }

    protected override async Task OnParametersSetAsync()
    {
        
        await base.OnParametersSetAsync();
    }

    #endregion
    
    

    
}