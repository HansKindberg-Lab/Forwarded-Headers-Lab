using System;
using System.Net;
using Newtonsoft.Json;

namespace Application.Models.Newtonsoft.Json.Serialization
{
	public class IpAddressJsonConverter : JsonConverter<IPAddress>
	{
		#region Methods

		public override IPAddress ReadJson(JsonReader reader, Type objectType, IPAddress existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, IPAddress value, JsonSerializer serializer)
		{
			if(writer == null)
				throw new ArgumentNullException(nameof(writer));

			if(value == null)
				writer.WriteNull();
			else
				writer.WriteValue(value.ToString());
		}

		#endregion
	}
}