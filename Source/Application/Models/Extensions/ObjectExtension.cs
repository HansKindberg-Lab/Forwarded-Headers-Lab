using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Models.Text.Json.Serialization;

namespace Application.Models.Extensions
{
	public static class ObjectExtension
	{
		#region Fields

		private static JsonSerializerOptions _jsonSerializerOptions;

		#endregion

		#region Properties

		private static JsonSerializerOptions JsonSerializerOptions
		{
			get
			{
				if(_jsonSerializerOptions == null)
				{
					var jsonSerializerOptions = new JsonSerializerOptions
					{
						DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
						TypeInfoResolver = new TypeInfoResolver(),
						WriteIndented = true
					};

					jsonSerializerOptions.Converters.Add(new IpAddressJsonConverter());
					jsonSerializerOptions.Converters.Add(new IpNetworkJsonConverter());

					_jsonSerializerOptions = jsonSerializerOptions;
				}

				return _jsonSerializerOptions;
			}
		}

		#endregion

		#region Methods

		public static string ToJson(this object instance)
		{
			if(instance == null)
				throw new ArgumentNullException(nameof(instance));

			var json = JsonSerializer.Serialize(instance, JsonSerializerOptions);

			return json;
		}

		#endregion
	}
}