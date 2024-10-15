using System.Text.Json.Serialization;
using LiteDB;
using Restaurant.DataAccess.Converters;

namespace Restaurant.DataAccess.Entities;

public class Ingredients
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    [JsonConverter(typeof(GenericConverter<float>))]
    public float Cost { get; set; }
    [JsonConverter(typeof(GenericConverter<float>))]
    public float Price { get; set; }
    
    public string? Unit { get; set; }
    
    [JsonConverter(typeof(GenericConverter<int>))]
    public int AvailableQuantity { get; set; }
    
    [JsonConverter(typeof(GenericConverter<int>))]
    public int QuantityAlert { get; set; }
    
    public string IconURL { get; set; }

    #region System Property

    public bool IsSync { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public override string ToString() => BsonMapper.Global.ToDocument(this).ToString();

    #endregion
    

}