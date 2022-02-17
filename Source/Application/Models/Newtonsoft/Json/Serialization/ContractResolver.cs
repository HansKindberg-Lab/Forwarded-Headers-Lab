using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Models.Newtonsoft.Json.Serialization
{
	public class ContractResolver : DefaultContractResolver
	{
		#region Methods

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			return base.CreateProperties(type, memberSerialization)
				.OrderBy(jsonProperty => jsonProperty.Order ?? int.MaxValue)
				.ThenBy(jsonProperty => jsonProperty.PropertyName)
				.ToList();
		}

		#endregion
	}
}