using System.Text.Json;
using System.Text.Json.Serialization;

namespace Restaurant.DataAccess.Converters;
public class GenericConverter<T> : JsonConverter<T> where T : IConvertible
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            return (T)Convert.ChangeType(stringValue, typeof(T));
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return (T)Convert.ChangeType(reader.GetDouble(), typeof(T));
        }

        throw new JsonException($"Cannot convert {reader.TokenType} to {typeof(T)}.");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(Convert.ToDouble(value));
    }
}

public class GenericConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(float) || typeToConvert == typeof(double) ||
               typeToConvert == typeof(int) || typeToConvert == typeof(DateTime);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type converterType = typeof(GenericConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}