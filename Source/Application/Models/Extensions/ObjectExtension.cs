using System;
using Application.Models.Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Application.Models.Extensions
{
	public static class ObjectExtension
	{
		#region Methods

		public static string ToJson(this object instance)
		{
			if(instance == null)
				throw new ArgumentNullException(nameof(instance));

			var jsonSerializerSettings = new JsonSerializerSettings()
			{
				ContractResolver = new ContractResolver(),
				NullValueHandling = NullValueHandling.Ignore
			};

			jsonSerializerSettings.Converters.Add(new IpAddressJsonConverter());

			var json = JsonConvert.SerializeObject(instance, Formatting.Indented, jsonSerializerSettings);

			return json;
		}

		#endregion
	}
}