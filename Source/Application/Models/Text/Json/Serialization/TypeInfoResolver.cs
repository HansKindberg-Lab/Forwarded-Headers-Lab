using System;
using System.Collections;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Application.Models.Text.Json.Serialization
{
	public class TypeInfoResolver : DefaultJsonTypeInfoResolver
	{
		#region Methods

		protected internal virtual void DoNotSerializeEmptyCollections(JsonTypeInfo jsonTypeInfo)
		{
			if(jsonTypeInfo == null)
				return;

			foreach(var property in jsonTypeInfo.Properties)
			{
				var propertyType = property.PropertyType;

				if(propertyType == typeof(string))
					continue;

				if(propertyType.GetInterface(nameof(IEnumerable)) == null)
					continue;

				property.ShouldSerialize = (_, propertyValue) => propertyValue is IEnumerable enumerable && enumerable.Cast<object>().Any();
			}
		}

		public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
		{
			var jsonTypeInfo = base.GetTypeInfo(type, options);

			this.SortProperties(jsonTypeInfo);
			//this.DoNotSerializeEmptyCollections(jsonTypeInfo);

			return jsonTypeInfo;
		}

		protected internal virtual void SortProperties(JsonTypeInfo jsonTypeInfo)
		{
			if(jsonTypeInfo is not { Kind: JsonTypeInfoKind.Object })
				return;

			var properties = jsonTypeInfo.Properties.OrderBy(property => property.Order).ThenBy(property => property.Name, StringComparer.Ordinal).ToList();

			jsonTypeInfo.Properties.Clear();

			foreach(var property in properties)
			{
				jsonTypeInfo.Properties.Add(property);
			}
		}

		#endregion
	}
}