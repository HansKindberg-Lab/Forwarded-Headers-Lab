using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Models.Text.Json.Serialization
{
	public abstract class TypeDescriptorJsonConverter<T> : JsonConverter<T>
	{
		#region Fields

		private static readonly TypeConverter _typeConverter = TypeDescriptor.GetConverter(typeof(T));

		#endregion

		#region Properties

		protected virtual TypeConverter TypeConverter => _typeConverter;

		#endregion

		#region Methods

		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			return (T)this.TypeConverter.ConvertFromString(reader.GetString());
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		{
			if(writer == null)
				throw new ArgumentNullException(nameof(writer));

			if(value == null)
				writer.WriteNullValue();
			else
				writer.WriteStringValue(this.TypeConverter.ConvertToString(value));
		}

		#endregion
	}
}