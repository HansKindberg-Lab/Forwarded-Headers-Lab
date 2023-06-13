using System;
using System.Collections.Generic;

namespace Application.Models.Views
{
	public class HomeViewModel
	{
		#region Properties

		/// <summary>
		/// Default forwarded-headers-options as json.
		/// </summary>
		public virtual string DefaultForwardedHeadersOptions { get; set; }

		public virtual bool ForwardedHeadersEnabled { get; set; }

		/// <summary>
		/// Forwarded-headers-options as json.
		/// </summary>
		public virtual string ForwardedHeadersOptions { get; set; }

		public virtual IDictionary<string, string> IpToHostNameMap { get; } = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		#endregion
	}
}