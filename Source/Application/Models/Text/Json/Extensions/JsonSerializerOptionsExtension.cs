using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Models.Text.Json.Serialization;

namespace Application.Models.Text.Json.Extensions
{
	public static class JsonSerializerOptionsExtension
	{
		#region Methods

		public static void SetDefaults(this JsonSerializerOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			options.Converters.Add(new IpAddressJsonConverter());
			options.Converters.Add(new IpNetworkJsonConverter());
			options.Converters.Add(new JsonStringEnumConverter());
			options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
			options.PropertyNameCaseInsensitive = true;
			options.TypeInfoResolver = new TypeInfoResolver();
			options.WriteIndented = true;
		}

		#endregion
	}
}