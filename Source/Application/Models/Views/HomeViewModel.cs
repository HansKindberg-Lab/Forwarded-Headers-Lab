namespace Application.Models.Views
{
	public class HomeViewModel
	{
		#region Properties

		/// <summary>
		/// Default forwarded-headers-options as json.
		/// </summary>
		public virtual string DefaultForwardedHeadersOptions { get; set; }

		/// <summary>
		/// Forwarded-headers-options as json.
		/// </summary>
		public virtual string ForwardedHeadersOptions { get; set; }

		#endregion
	}
}