using System.Text.Json.Serialization;
using System.Text.Json;

namespace TaSked.Application.Common;

public class OptionalConverter : JsonConverter<Optional<object>>
{
	public override Optional<object> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return new Optional<object>(null);
		}

		var value = JsonSerializer.Deserialize<object>(ref reader, options);
		return new Optional<object>(value);
	}

	public override void Write(Utf8JsonWriter writer, Optional<object> value, JsonSerializerOptions options)
	{
		if (value.TryGetValue(out var optionalValue))
		{
			JsonSerializer.Serialize(writer, optionalValue, options);
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
