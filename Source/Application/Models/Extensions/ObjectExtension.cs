using System;
using System.Text.Json;
using Application.Models.Text.Json.Extensions;

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
					var jsonSerializerOptions = new JsonSerializerOptions();

					jsonSerializerOptions.SetDefaults();

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