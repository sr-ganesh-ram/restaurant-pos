using LiteDB;

namespace Restaurant.Template.Models;

public class GeneralSettings
{
    public int Id { get; set; }
    public string AppURL { get; set; }
    public string AppName { get; set; }
    public string ContactPhone { get; set; }
    public string Address { get; set; }
    public bool IsSync { get; set; } = false;
    public override string ToString() => BsonMapper.Global.ToDocument(this).ToString();
}