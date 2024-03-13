using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace TaSked.Api.ApiClient;

// Needed to deserialize domain objects with private constructors and properties
internal class PrivateConstructorDeserializationModifier 
{
	public static void UsePrivateConstructor(JsonTypeInfo jsonTypeInfo)
	{
		if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object && jsonTypeInfo.CreateObject is null)
		{
			if (jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0)
			{
				jsonTypeInfo.CreateObject = () =>
					Activator.CreateInstance(jsonTypeInfo.Type, true);

				foreach (PropertyInfo property in jsonTypeInfo.Type.GetProperties())
				{
					if (property.SetMethod?.IsPublic ?? true)
					{
						continue;
					}

					JsonPropertyInfo jsonPropertyInfo;

					jsonPropertyInfo = jsonTypeInfo.Properties
						.FirstOrDefault(prop => prop.Name.ToLower() == property.Name.ToLower()
						, jsonTypeInfo.CreateJsonPropertyInfo(property.PropertyType, property.Name));
					
					jsonPropertyInfo.Set = property.SetValue;
				}
			}
		}
	}
}
