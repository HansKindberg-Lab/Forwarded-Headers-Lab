using System.Net;

namespace Application.Models.Net
{
	public interface IDnsResolver
	{
		#region Methods

		string GetHostName(IPAddress ipAddress);

		#endregion
	}
}